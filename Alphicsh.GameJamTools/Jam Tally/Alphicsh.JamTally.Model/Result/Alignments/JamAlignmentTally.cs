using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Alignments
{
    public class JamAlignmentTally
    {
        public IReadOnlyCollection<JamAlignmentVote> Votes { get; init; } = default!;
        public IReadOnlyDictionary<JamAlignmentOption, decimal> BaseScores { get; init; } = default!;
        public IReadOnlyDictionary<JamAlignmentOption, JamAlignmentReviewScore> ReviewScores { get; init; } = default!;
        public IReadOnlyDictionary<JamAlignmentOption, decimal> ThemeBonuses { get; init; } = default!;
        public IReadOnlyDictionary<JamAlignmentOption, decimal> TotalScores { get; init; } = default!;

        public IReadOnlyCollection<JamAlignmentOption> AlignmentRanking { get; init; } = default!;
    }
}
