using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTools.Common.IO.Jam.New.Entries;

namespace Alphicsh.JamTools.Common.IO.Jam.New
{
    public class NewJamInfo
    {
        public FilePath Location { get; internal set; } = default!;
        
        public string Version { get; init; } = default!;

        // -------------------
        // General information
        // -------------------

        public string? Title { get; init; } = default!;
        public string? LogoFileName { get; set; } = default!;
        public string? Theme { get; init; } = default!;
        public IReadOnlyCollection<NewJamAwardInfo> AwardCriteria { get; init; } = default!;
        public NewJamAlignmentInfo? Alignments { get; init; } = default!;

        // -------
        // Entries
        // -------

        public string EntriesSubpath { get; init; } = default!;
        public IReadOnlyCollection<NewJamEntryInfo> Entries { get; init; } = default!;

        public NewJamInfo()
        {
        }

        public NewJamInfo(NewJamCore core, IEnumerable<NewJamEntryInfo> entries)
        {
            Location = core.Location;
            Version = core.Version;

            Title = core.Title;
            LogoFileName = core.LogoFileName;
            Theme = core.Theme;
            AwardCriteria = core.AwardCriteria;
            Alignments = core.Alignments;

            EntriesSubpath = core.EntriesSubpath;
            Entries = entries.ToList();
        }
    }
}
