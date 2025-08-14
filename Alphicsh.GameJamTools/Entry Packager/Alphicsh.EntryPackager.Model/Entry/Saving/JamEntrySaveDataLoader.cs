using Alphicsh.JamTools.Common.IO.Jam.New.Entries;
using Alphicsh.JamTools.Common.IO.Saving;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.EntryPackager.Model.Entry.Saving
{
    public class JamEntrySaveDataLoader : ISaveDataLoader<JamEntryEditable, JamEntrySaveData>
    {
        private JsonFileLoader<NewJamEntryInfo> EntryLoader { get; } = new JsonFileLoader<NewJamEntryInfo>();

        public JamEntrySaveData? Load(JamEntryEditable model)
        {
            var directoryPath = model.Files.DirectoryPath;
            var entryPath = directoryPath.Append("entry.jamentry");
            var entryInfo = EntryLoader.TryLoad(entryPath);
            if (entryInfo == null)
                return null;

            return new JamEntrySaveData { DirectoryPath = directoryPath, EntryInfo = entryInfo };
        }
    }
}
