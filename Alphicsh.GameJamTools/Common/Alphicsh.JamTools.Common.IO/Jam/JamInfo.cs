using System.Collections.Generic;

namespace Alphicsh.JamTools.Common.IO.Jam
{
    public class JamInfo
    {
        public string Version { get; init; } = default!;

        // -------------------
        // General information
        // -------------------

        public string? Title { get; init; } = default!;
        public string? LogoFileName { get; set; } = default!;
        public string? Theme { get; init; } = default!;
        public IReadOnlyCollection<JamAwardInfo> AwardCriteria { get; init; } = default!;
        public JamAlignmentInfo? Alignments { get; init; } = default!;

        public string EntriesSubpath { get; init; } = default!;
        public IReadOnlyCollection<JamEntryStub> Entries { get; init; } = default!;
    }
}
