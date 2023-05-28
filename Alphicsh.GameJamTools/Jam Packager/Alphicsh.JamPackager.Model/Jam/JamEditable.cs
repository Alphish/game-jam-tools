using System.Collections.Generic;
using Alphicsh.EntryPackager.Model.Entry;
using Alphicsh.JamPackager.Model.Jam.Loading;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPackager.Model.Jam
{
    public class JamEditable
    {
        private static JamEntriesExplorer EntriesExplorer { get; } = new JamEntriesExplorer();

        public FilePath DirectoryPath { get; init; }
        public string? Title { get; set; } = default!;
        public string? Theme { get; set; } = default!;
        public IList<JamAwardEditable> Awards { get; } = new List<JamAwardEditable>();

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
