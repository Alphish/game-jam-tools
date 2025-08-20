using System.Collections.Generic;
using System.Linq;
using Alphicsh.EntryPackager.Model.Entry;
using Alphicsh.JamPackager.Model.Jam.Loading;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPackager.Model.Jam
{
    public class JamEditable
    {
        private static JamEntriesExplorer EntriesExplorer { get; } = new JamEntriesExplorer();

        public FilePath DirectoryPath { get; init; }
        public string? Title { get; set; }
        public string? Theme { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }

        public IList<string> Hosts { get; set; } = new List<string>();

        public string? GetHostsString()
            => Hosts.Count == 0 ? null : string.Join(", ", Hosts);

        public void SetHostsString(string? hostsString)
        {
            Hosts = !string.IsNullOrWhiteSpace(hostsString)
                ? hostsString.Split(',').Select(name => name.Trim()).ToList()
                : new List<string>();
        }

        public IList<JamLinkEditable> Links { get; set; } = new List<JamLinkEditable>();

        public IList<JamAwardEditable> Awards { get; set; } = new List<JamAwardEditable>();

        // Entries finding

        public string EntriesLocation { get; private set; } = default!;
        public IReadOnlyCollection<JamEntryEditable> Entries { get; private set; } = default!;
        public void SetEntriesPath(FilePath entriesPath)
        {
            if (!entriesPath.IsSubpathOf(DirectoryPath))
                return;

            EntriesLocation = entriesPath.AsRelativeTo(DirectoryPath).Value;
            Entries = EntriesExplorer.FindEntries(entriesPath);
        }

        // Logo handling

        public string? LogoLocation { get; set; } = default;
        public FilePath? LogoPath => DirectoryPath.AppendNullable(LogoLocation);
    }
}
