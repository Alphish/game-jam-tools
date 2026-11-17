using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result.Calculation
{
    public class JamTallyCalculator
    {
        private JamTallyRankScoreCalculator MainScoreCalculator { get; } = new JamTallyRankScoreCalculator(entryCounter => entryCounter.Ranks.Sum(rank => 1m / (rank + 1)));

        public JamTallyResult Calculate(JamOverview jam, IReadOnlyList<JamVote> votes)
        {
            var tallyVotes = votes.Select(JamTallyVote.ForMainTally).ToList();

            var ranking = MainScoreCalculator.TallyRanking(jam.Entries, tallyVotes);
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
    }
}
