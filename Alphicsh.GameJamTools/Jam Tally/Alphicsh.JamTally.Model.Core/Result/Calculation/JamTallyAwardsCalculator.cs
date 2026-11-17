using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Calculation
{
    internal class JamTallyAwardsCalculator
    {
        public JamTallyAwardsSummary CalculateAwards(IReadOnlyList<JamAwardCriterion> awardCriteria, IReadOnlyList<JamTallyEntry> entrySummaries)
        {
            var awardEntryScores = entrySummaries
                .SelectMany(entry => entry.AwardScores)
                .GroupBy(awardScore => awardScore.Criterion)
                .ToDictionary(
                    group => group.Key,
                    awardScores => awardScores.OrderByDescending(awardScore => awardScore.Score).ThenBy(awardScore => awardScore.Entry.TallyTitle).ToList() as IReadOnlyList<JamTallyAwardScore>
                    );

            var topScores = awardEntryScores.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.First().Score);
            var awardWinners = awardEntryScores.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Where(awardScore => awardScore.Score == topScores[kvp.Key]).Select(awardScore => awardScore.Entry).ToList() as IReadOnlyList<JamEntry>
                );

            var entrySummariesByEntry = entrySummaries.ToDictionary(tallyEntry => tallyEntry.Entry);
            foreach (var criterion in awardCriteria)
            {
                foreach (var awardedEntry in awardWinners[criterion])
                {
                    entrySummariesByEntry[awardedEntry].Awards.Add(criterion);
                }
            }

            return new JamTallyAwardsSummary
            {
                AwardCriteria = awardCriteria,
                AwardEntryScores = awardEntryScores,
                AwardTopScores = topScores,
                AwardWinners = awardWinners,
            };
        }
    }
}
