using System.Collections.Generic;
using Alphicsh.EntryPackager.Model.Entry;
using Alphicsh.JamPackager.Model.Jam.Loading;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPackager.Model.Jam
{
    public class JamEditable
    {
        private static JamExplorer Explorer { get; } = new JamExplorer();

        public FilePath DirectoryPath { get; init; }
        public string Title { get; set; } = default!;
        public string Theme { get; set; } = default!;
        public ICollection<JamAwardEditable> Awards { get; } = new List<JamAwardEditable>();

        // Entries finding

        public string EntriesLocation { get; private set; } = default!;
        public IReadOnlyCollection<JamEntryEditable> Entries { get; private set; }
        public void SetEntriesPath(FilePath entriesPath)
        {
            if (!entriesPath.IsSubpathOf(DirectoryPath))
                return;

            EntriesLocation = entriesPath.AsRelativeTo(DirectoryPath).Value;
            Entries = Explorer.FindEntries(entriesPath);
        }
    }
}
