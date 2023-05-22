﻿using Alphicsh.EntryPackager.Model.Entry;
using Alphicsh.EntryPackager.Model.Entry.Loading;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.EntryPackager.Model
{
    public class AppModel
    {
        private JamEntryExplorer EntryExplorer { get; } = new JamEntryExplorer();

        public JamEntryEditable? Entry { get; private set; }
        public bool HasEntry => Entry != null;

        public AppModel()
        {
        }

        public void LoadDirectory(FilePath directoryPath)
            => Entry = EntryExplorer.LoadFromDirectory(directoryPath);

        public void LoadEntryInfo(FilePath entryInfoPath)
            => Entry = EntryExplorer.LoadFromFile(entryInfoPath);

        public void LoadEntryZip(FilePath zipPath)
            => Entry = EntryExplorer.LoadFromZip(zipPath);
    }
}
