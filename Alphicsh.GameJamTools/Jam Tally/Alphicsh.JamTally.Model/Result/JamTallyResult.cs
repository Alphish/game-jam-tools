using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result
{
    public class JamTallyResult
    {
        public IReadOnlyCollection<JamAwardCriterion> Awards { get; init; } = default!;
        public IReadOnlyCollection<JamEntry> Entries { get; init; } = default!;
        public IReadOnlyCollection<JamVote> Votes { get; init; } = default!;

        // -------
        // Ranking
        // -------

        public IReadOnlyCollection<JamTallyEntryScore> FinalRanking { get; init; } = default!;
        public string GetFinalRankingText()
            => string.Join("\n", FinalRanking.Select(score => score.ToString()));

        // ------
        // Awards
        // ------

        public IReadOnlyCollection<JamTallyAwardRanking> AwardRankings { get; init; } = default!;
        public string GetAwardRankingsText()
        {
            var lines = AwardRankings.SelectMany(ranking => GetAwardRankingLines(ranking));
            return string.Join("\n", lines);
        }
        private IEnumerable<string> GetAwardRankingLines(JamTallyAwardRanking ranking)
        {
            yield return ranking.Award.Name + ":";

            foreach (var score in ranking.Scores)
                yield return $"{score.Entry.Line}: {score.Count}";

            yield return "";
        }

    }
}
