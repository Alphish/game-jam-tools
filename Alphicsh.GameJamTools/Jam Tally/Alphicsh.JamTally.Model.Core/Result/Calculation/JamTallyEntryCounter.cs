using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Calculation
{
    public class JamTallyEntryCounter
    {
        public JamEntry Entry { get; init; } = default!;

        public IList<int> Ranks { get; } = new List<int>();
        public int UnjudgedCount { get; private set; } = 0;
        public int MissingCount { get; private set; } = 0;
        public IDictionary<JamAwardCriterion, int> AwardCounts { get; set; } = new Dictionary<JamAwardCriterion, int>();

        public void CountRank(int rank) => Ranks.Add(rank);
        public void CountAuthored() => UnjudgedCount++;
        public void CountUnjudged() => UnjudgedCount++;
        public void CountMissing() => MissingCount++;

        public void CountAward(JamAwardCriterion criterion)
        {
            if (!AwardCounts.ContainsKey(criterion))
                AwardCounts[criterion] = 0;

            AwardCounts[criterion] += 1;
        }
    }
}
