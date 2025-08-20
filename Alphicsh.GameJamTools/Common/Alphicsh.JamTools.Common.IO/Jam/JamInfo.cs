using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTools.Common.IO.Jam.Entries;

namespace Alphicsh.JamTools.Common.IO.Jam
{
    public class JamInfo
    {
        public FilePath Location { get; internal set; } = default!;

        public string Version { get; init; } = default!;

        // -------------------
        // General information
        // -------------------

        public string? Title { get; init; } = default!;
        public string? LogoFileName { get; set; } = default!;
        public string? Theme { get; init; } = default!;
        public string? StartTime { get; init; } = default!;
        public string? EndTime { get; init; } = default!;
        public IReadOnlyCollection<string> Hosts { get; init; } = new List<string>();
        public IReadOnlyCollection<JamLink> Links { get; init; } = new List<JamLink>();

        public IReadOnlyCollection<JamAwardInfo> AwardCriteria { get; init; } = default!;
        public JamAlignmentInfo? Alignments { get; init; } = default!;

        // -------
        // Entries
        // -------

        public string EntriesSubpath { get; init; } = default!;
        public IReadOnlyCollection<JamEntryInfo> Entries { get; init; } = default!;

        public JamInfo()
        {
        }

        public JamInfo(JamCore core, IEnumerable<JamEntryInfo> entries)
        {
            Location = core.Location;
            Version = core.Version;

            Title = core.Title;
            LogoFileName = core.LogoFileName;
            Theme = core.Theme;
            StartTime = core.StartTime;
            EndTime = core.EndTime;
            Hosts = core.Hosts;
            Links = core.Links;

            AwardCriteria = core.AwardCriteria;
            Alignments = core.Alignments;

            EntriesSubpath = core.EntriesSubpath;
            Entries = entries.ToList();
        }
    }
}
