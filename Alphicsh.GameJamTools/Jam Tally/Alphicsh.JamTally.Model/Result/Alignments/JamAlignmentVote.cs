using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result.Alignments
{
    public class JamAlignmentVote
    {
        public JamVote OriginalVote { get; init; } = default!;
        public string? Voter => OriginalVote.Voter;
        public JamAlignmentOption? VoterAlignment => OriginalVote.Alignment;

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
