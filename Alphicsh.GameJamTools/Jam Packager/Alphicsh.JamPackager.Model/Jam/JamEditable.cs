using System.Collections.Generic;
using System.IO;
using System.Linq;
using Alphicsh.EntryPackager.Model.Entry;
using Alphicsh.EntryPackager.Model.Entry.Export;
using Alphicsh.JamPackager.Model.Jam.Loading;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Execution;

namespace Alphicsh.JamPackager.Model.Jam
{
    public class JamEditable
    {
        private static JamEntriesExplorer EntriesExplorer { get; } = new JamEntriesExplorer();
        private static PackageNameFormatter EntryNameFormatter { get; } = new PackageNameFormatter();

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
        public IList<JamEntryEditable> Entries { get; private set; } = default!;
        public void SetEntriesPath(FilePath entriesPath)
        {
            if (!entriesPath.IsSubpathOf(DirectoryPath))
                return;

            EntriesLocation = entriesPath.AsRelativeTo(DirectoryPath).Value;
            Entries = EntriesExplorer.FindEntries(entriesPath).ToList();
        }

        public JamEntryEditable AddWebEntry(string title, string? shortTitle, string? team, string authors, string url)
        {
            var entry = new JamEntryEditable { Title = title, ShortTitle = shortTitle };
            if (team != null)
                entry.Team.Name = team;

            entry.Team.SetAuthorsString(authors);

            var launcher = entry.Files.AddLauncher();
            launcher.SetName("Web");
            launcher.SetType(LaunchType.WebLink);
            launcher.SetLocation(url);

            var folderName = EntryNameFormatter.GetPackageName(entry.DisplayShortTitle, entry.Team.DisplayName);
            var folderPath = DirectoryPath.Append(EntriesLocation).Append(folderName);
            Directory.CreateDirectory(folderPath.Value);

            entry.Files.SetDirectoryPath(folderPath);

            Entries.Add(entry);

            return entry;
        }

        // Logo handling

        public string? LogoLocation { get; set; } = default;
        public FilePath? LogoPath => DirectoryPath.AppendNullable(LogoLocation);
    }
}
