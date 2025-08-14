using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alphicsh.JamTools.Common.IO.Jam
{
    public class JamCore
    {
        [JsonIgnore] public FilePath Location { get; internal set; } = default!;
        [JsonIgnore] public FilePath Directory => Location.GetParentDirectoryPath();

        public string Version { get; init; } = default!;

        // -------------------
        // General information
        // -------------------

        public string? Title { get; init; } = default!;
        public string? LogoFileName { get; set; } = default!;
        public string? Theme { get; init; } = default!;
        public IReadOnlyCollection<JamAwardInfo> AwardCriteria { get; init; } = default!;
        public JamAlignmentInfo? Alignments { get; init; } = default!;

        // -------
        // Entries
        // -------

        public string EntriesSubpath { get; init; } = default!;
        public IReadOnlyCollection<JamEntryStub> Entries { get; init; } = default!;
    }
}
