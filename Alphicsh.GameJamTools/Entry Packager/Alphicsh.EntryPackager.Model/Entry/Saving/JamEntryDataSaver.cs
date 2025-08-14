using Alphicsh.JamTools.Common.IO.Jam.Entries;
using Alphicsh.JamTools.Common.IO.Saving;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.EntryPackager.Model.Entry.Saving
{
    public class JamEntryDataSaver : IDataSaver<JamEntrySaveData>
    {
        private JsonFileSaver<JamEntryInfo> Saver { get; } = new JsonFileSaver<JamEntryInfo>();

        public void Save(JamEntrySaveData saveData)
        {
            var entryInfoPath = saveData.DirectoryPath.Append("entry.jamentry");
            Saver.Save(entryInfoPath, saveData.EntryInfo);
        }
    }
}
