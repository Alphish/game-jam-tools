using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Alignments
{
    public class JamAlignmentVote
    {
        public int TotalRankedCount { get; init; } = default!;
        public int TotalUnrankedCount { get; init; } = default!;
        public int TotalEntriesCount { get; init; } = default!;

        public IReadOnlyDictionary<JamAlignmentOption, int> RankedEntriesCounts { get; init; } = default!;
        public IReadOnlyDictionary<JamAlignmentOption, decimal> RankedAverages { get; init; } = default!;
        public IReadOnlyDictionary<JamAlignmentOption, int> UnrankedEntriesCounts { get; init; } = default!;
        public IReadOnlyDictionary<JamAlignmentOption, decimal> UnrankedAverages { get; init; } = default!;
        public IReadOnlyDictionary<JamAlignmentOption, int> AlignedEntriesCounts { get; init; } = default!;
        public IReadOnlyDictionary<JamAlignmentOption, decimal> AlignedAverages { get; init; } = default!;

        public IReadOnlyCollection<JamAlignmentEntryScore> EntryScores { get; init; } = default!;
    }
}
