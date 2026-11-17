using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result.Calculation
{
    public class JamTallyCalculator
    {
        public JamTallyResult Calculate(JamOverview jam, JamVoteCollection votes)
        {
            var tallyVotes = GetTallyVotes(votes);
            var ranking = TallyRanking(jam.Entries, tallyVotes);
            var rankingByEntry = ranking.ToDictionary(tallyEntry => tallyEntry.Entry);

            var awardEntryScores = ranking
                .SelectMany(entry => entry.AwardScores)
                .GroupBy(awardScore => awardScore.Criterion)
                .ToDictionary(
                    group => group.Key,
                    awardScores => awardScores.OrderByDescending(awardScore => awardScore.Score).ThenBy(awardScore => awardScore.Entry.TallyTitle).ToList() as IReadOnlyList<JamTallyAwardScore>
                    );

            var topScores = awardEntryScores.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.First().Score);
            var awardEntries = awardEntryScores.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Where(awardScore => awardScore.Score == topScores[kvp.Key]).Select(awardScore => awardScore.Entry).ToList() as IReadOnlyList<JamEntry>
                );

            foreach (var criterion in jam.AwardCriteria)
            {
                foreach (var awardedEntry in awardEntries[criterion])
                {
                    rankingByEntry[awardedEntry].Awards.Add(criterion);
                }
            }

            var topReviewScore = tallyVotes.Max(vote => vote.ReviewScore);
            var topReviewers = tallyVotes.Where(vote => vote.ReviewScore == topReviewScore).Select(vote => vote.Voter).ToList();

            return new JamTallyResult
            {
                Ranking = ranking,
                AwardCriteria = jam.AwardCriteria.ToList(),
                AwardEntryScores = awardEntryScores,
                AwardTopScores = topScores,
                AwardWinners = awardEntries,
                Votes = tallyVotes,
                TopReviewScore = topReviewScore,
                TopReviewers = topReviewers,
            };
        }

        // -----
        // Votes
        // -----

        private IReadOnlyList<JamTallyVote> GetTallyVotes(JamVoteCollection votesCollection)
        {
            return votesCollection.Votes.Where(vote => vote.Ranking.Any()).Select(GetTallyVote).ToList();
        }

        private JamTallyVote GetTallyVote(JamVote originalVote)
        {
            var reactions = originalVote.AggregateReactions.OrderBy(reaction => reaction.User).ToList();
            return new JamTallyVote
            {
                Voter = originalVote.Voter,
                VoterCode = ExtractVoterCode(originalVote.Voter),
                VoterAlignment = originalVote.Alignment,
                Ranking = originalVote.Ranking.ToList(),
                Unjudged = originalVote.Unjudged.Union(originalVote.Authored).ToList(),
                Awards = originalVote.Awards.ToDictionary(award => award.Criterion, award => award.Entry),
                Reactions = reactions,
                ReviewScore = reactions.Sum(reaction => reaction.Value),
            };
        }

        private string ExtractVoterCode(string title)
        {
            var codeBuilder = new StringBuilder();
            foreach (char c in title)
            {
                if (c >= 128)
                    continue;

                if (char.IsLetterOrDigit(c))
                    codeBuilder.Append(c);
                else if (codeBuilder.Length > 0 && codeBuilder[^1] != '_')
                    codeBuilder.Append('_');
            }
            return codeBuilder.ToString().ToLowerInvariant();
        }

        // -------
        // Entries
        // -------

        private IReadOnlyList<JamTallyEntry> TallyRanking(IReadOnlyCollection<JamEntry> entries, IReadOnlyCollection<JamTallyVote> votes)
        {
            var entryCounters = entries.Select(entry => new JamTallyEntryCounter(entry, votes.Count)).ToDictionary(counter => counter.Entry);

            foreach (var vote in votes)
            {
                for (var i = 0; i < vote.Ranking.Count; i++)
                {
                    var entry = vote.Ranking[i];
                    var rank = i + 1;
                    var entryCounter = entryCounters[entry];
                    entryCounter.CountRank(rank);
                }

                foreach (var entry in vote.Unjudged)
                {
                    var entryCounter = entryCounters[entry];
                    entryCounter.CountUnjudged();
                }

                foreach (var award in vote.Awards)
                {
                    var criterion = award.Key;
                    var entry = award.Value;

                    var entryCounter = entryCounters[entry];
                    entryCounter.CountAward(criterion);
                }
            }

            var fullRanking = entryCounters.Values
                .Select(counter => counter.CompleteTally())
                .OrderByDescending(tally => tally.TotalScore)
                .ToList();

            for (var i = 0; i < fullRanking.Count; i++)
            {
                fullRanking[i].Rank = i + 1;
            }

            return fullRanking;
        }
    }
}
