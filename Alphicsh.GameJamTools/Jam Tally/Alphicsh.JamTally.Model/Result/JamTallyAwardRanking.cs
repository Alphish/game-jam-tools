using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result
{
    public class JamTallyAwardRanking
    {
        public JamAwardCriterion Award { get; init; } = default!;
        public IReadOnlyCollection<JamTallyAwardScore> Scores { get; init; } = default!;

        public int GetMaxScore()
            => Scores.Max(score => score.Count);

        public IReadOnlyCollection<JamTallyAwardScore> GetWinners()
        {
            var maxScore = GetMaxScore();
            return Scores.Where(score => score.Count == maxScore).ToList();
        }
    }
}
