using System;
using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Calculation
{
    public class JamTallyRankScoreCalculator
    {
        private Func<JamTallyEntryCounter, decimal> BaseScoreCalculation { get; }

        public JamTallyRankScoreCalculator(Func<JamTallyEntryCounter, decimal> baseScoreCalculation)
        {
            BaseScoreCalculation = baseScoreCalculation;
        }

        public IReadOnlyList<JamTallyEntry> TallyRanking(IReadOnlyList<JamEntry> entries, IReadOnlyList<JamTallyVote> votes)
        {
            var entryCounters = CountEntries(entries, votes);

            var fullRanking = entryCounters
                .Select(CalculateEntrySummary)
                .OrderByDescending(tally => tally.TotalScore)
                .ToList();

            for (var i = 0; i < fullRanking.Count; i++)
            {
                fullRanking[i].Rank = i + 1;
            }

            return fullRanking;
        }

        private IReadOnlyList<JamTallyEntryCounter> CountEntries(IReadOnlyList<JamEntry> entries, IReadOnlyList<JamTallyVote> votes)
        {
            var entryCounters = entries.Select(entry => new JamTallyEntryCounter { Entry = entry }).ToDictionary(counter => counter.Entry);

            foreach (var vote in votes)
            {
                for (var i = 0; i < vote.Ranking.Count; i++)
                {
                    var entry = vote.Ranking[i];
                    entryCounters[entry].CountRank(i + 1);
                }
                foreach (var entry in vote.Authored)
                {
                    entryCounters[entry].CountUnjudged();
                }
                foreach (var entry in vote.Unjudged)
                {
                    entryCounters[entry].CountUnjudged();
                }
                foreach (var award in vote.Awards)
                {
                    var criterion = award.Key;
                    var entry = award.Value;
                    entryCounters[entry].CountAward(criterion);
                }
            }

            return entries.Select(entry => entryCounters[entry]).ToList();
        }

        private JamTallyEntry CalculateEntrySummary(JamTallyEntryCounter entryCounter)
        {
            var judgedCount = entryCounter.Ranks.Count + entryCounter.MissingCount;
            var unjudgedCount = entryCounter.UnjudgedCount;
            var totalCount = judgedCount + unjudgedCount;
            var baseScore = BaseScoreCalculation(entryCounter);
            var totalScore = (baseScore * totalCount) / judgedCount;

            return new JamTallyEntry
            {
                Entry = entryCounter.Entry,
                JudgedCount = judgedCount,
                UnjudgedCount = unjudgedCount,
                BaseScore = baseScore,
                TotalScore = totalScore,
                AwardScores = entryCounter.AwardCounts
                    .Select(kvp => new JamTallyAwardScore { Criterion = kvp.Key, Entry = entryCounter.Entry, Score = kvp.Value })
                    .ToList(),
            };
        }
    }
}
