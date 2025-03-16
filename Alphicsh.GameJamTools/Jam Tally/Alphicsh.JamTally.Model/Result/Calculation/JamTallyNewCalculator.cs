using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result.Calculation
{
    public class JamTallyNewCalculator
    {
        public JamTallyNewResult Calculate(JamOverview jam, JamVoteCollection votes)
        {
            var tallyVotes = GetTallyVotes(votes);
            var ranking = TallyRanking(jam.Entries, tallyVotes);

            var topScores = GetAwardTopScores(ranking);
            var awardEntries = GetAwardEntries(topScores, ranking);

            var topReviewScore = GetTopReviewScore(tallyVotes);
            var topReviewers = GetTopReviewers(topReviewScore, tallyVotes);

            return new JamTallyNewResult
            {
                Ranking = ranking,
                AwardCriteria = jam.AwardCriteria.ToList(),
                AwardTopScores = topScores,
                AwardEntries = awardEntries,
                Votes = tallyVotes,
                TopReviewScore = topReviewScore,
                TopReviewers = topReviewers,
            };
        }

        // -----
        // Votes
        // -----

        private IReadOnlyCollection<JamTallyVote> GetTallyVotes(JamVoteCollection votesCollection)
        {
            return votesCollection.Votes.Where(vote => vote.Ranking.Any()).Select(GetTallyVote).ToList();
        }

        private JamTallyVote GetTallyVote(JamVote originalVote)
        {
            var reactions = originalVote.AggregateReactions.OrderBy(reaction => reaction.User).ToList();
            return new JamTallyVote
            {
                Voter = originalVote.Voter,
                VoterAlignment = originalVote.Alignment,
                Ranking = originalVote.Ranking.ToList(),
                Unjudged = originalVote.Unjudged.Union(originalVote.Authored).ToList(),
                Awards = originalVote.Awards.ToDictionary(award => award.Criterion, award => award.Entry),
                Reactions = reactions,
                ReviewScore = reactions.Sum(reaction => reaction.Value),
            };
        }

        // -------
        // Entries
        // -------

        private IReadOnlyCollection<JamTallyEntry> TallyRanking(IReadOnlyCollection<JamEntry> entries, IReadOnlyCollection<JamTallyVote> votes)
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

            return entryCounters.Values
                .Select(counter => counter.CompleteTally())
                .OrderByDescending(tally => tally.TotalScore)
                .ToList();
        }

        // ------
        // Awards
        // ------

        private IReadOnlyDictionary<JamAwardCriterion, int> GetAwardTopScores(IReadOnlyCollection<JamTallyEntry> entries)
        {
            return entries.SelectMany(entry => entry.AwardScores)
                .GroupBy(score => score.Criterion)
                .ToDictionary(group => group.Key, scores => scores.Max(awardScore => awardScore.Score));
        }

        private IReadOnlyDictionary<JamAwardCriterion, IReadOnlyCollection<JamEntry>> GetAwardEntries(IReadOnlyDictionary<JamAwardCriterion, int> topScores, IReadOnlyCollection<JamTallyEntry> entries)
        {
            var awardEntries = topScores.Keys.ToDictionary(criterion => criterion, criterion => new List<JamEntry>());

            var allScores = entries.SelectMany(entry => entry.AwardScores);
            foreach (var awardScore in allScores)
            {
                if (awardScore.Score < topScores[awardScore.Criterion])
                    continue;

                awardEntries[awardScore.Criterion].Add(awardScore.Entry);
            }

            return awardEntries.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.OrderBy(entry => entry.TallyTitle).ToList() as IReadOnlyCollection<JamEntry>
                );
        }

        private int GetTopReviewScore(IReadOnlyCollection<JamTallyVote> votes)
            => votes.Max(vote => vote.ReviewScore);

        private IReadOnlyCollection<string> GetTopReviewers(int topScore, IReadOnlyCollection<JamTallyVote> votes)
            => votes.Where(vote => vote.ReviewScore == topScore).Select(vote => vote.Voter).ToList();
    }
}
