using System.Collections.Generic;

namespace Alphicsh.JamTally.Model.Result
{
    public class JamTallyAlignmentResult
    {
        public IReadOnlyList<JamTallyEntry> Entries { get; init; } = default!;
        public IReadOnlyList<JamTallyAlignment> Ranking { get; init; } = default!;
        public IReadOnlyCollection<JamTallyVote> Votes { get; init; } = default!;
    }
}
