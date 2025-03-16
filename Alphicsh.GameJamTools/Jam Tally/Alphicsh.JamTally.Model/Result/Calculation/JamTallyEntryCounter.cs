using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Calculation
{
    public class JamTallyEntryCounter
    {
        public JamTallyEntryCounter(JamEntry entry, int votesCount)
        {
            Entry = entry;
            VotesCount = votesCount;
        }

        public JamEntry Entry { get; }
        private int VotesCount { get; }

        private int UnjudgedCount { get; set; } = 0;
        private decimal BaseScore { get; set; } = 0m;
        private IDictionary<JamAwardCriterion, int> AwardCounts { get; set; } = new Dictionary<JamAwardCriterion, int>();

        public void CountRank(int rank)
            => BaseScore += 1m / (rank + 1);

        public void CountUnjudged()
            => UnjudgedCount++;

        public void CountAward(JamAwardCriterion criterion)
        {
            if (!AwardCounts.ContainsKey(criterion))
                AwardCounts[criterion] = 0;

            AwardCounts[criterion] += 1;
        }

        public JamTallyEntry CompleteTally()
        {
            var judgedCount = VotesCount - UnjudgedCount;
            return new JamTallyEntry
            {
                Entry = Entry,

                TotalVotesCount = VotesCount,
                JudgedCount = judgedCount,
                UnjudgedCount = UnjudgedCount,
                BaseScore = BaseScore,
                TotalScore = VotesCount * BaseScore / judgedCount,

                AwardScores = AwardCounts.Select(award => new JamTallyNewAwardScore { Entry = Entry, Criterion = award.Key, Score = award.Value }).ToList()
            };
        }
    }
}
