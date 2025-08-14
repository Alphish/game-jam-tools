using System.Linq;
using Alphicsh.EntryPackager.Model.Entry.Saving;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Jam.New;
using Alphicsh.JamTools.Common.IO.Jam.New.Entries;
using Alphicsh.JamTools.Common.IO.Saving;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.JamPackager.Model.Jam.Saving
{
    public class JamSaveDataLoader : ISaveDataLoader<JamEditable, JamSaveData>
    {
        private static JsonFileLoader<NewJamCore> JamInfoLoader { get; } = new JsonFileLoader<NewJamCore>();
        private JsonFileLoader<NewJamEntryInfo> EntryLoader { get; } = new JsonFileLoader<NewJamEntryInfo>();

        public JamSaveData? Load(JamEditable model)
        {
            var jamInfoPath = model.DirectoryPath.Append("jam.jaminfo");
            var jamInfo = JamInfoLoader.TryLoad(jamInfoPath);
            if (jamInfo == null)
                return null;

            var entriesPath = model.DirectoryPath.Append(jamInfo.EntriesSubpath);
            var entriesData = jamInfo.Entries
                .Select(stub => LoadEntryData(entriesPath, stub))
                .Where(data => data != null)
                .Select(data => data!)
                .ToList();
            if (entriesData.Any(entry => entry == null))
                return null;

            return new JamSaveData { DirectoryPath = model.DirectoryPath, JamInfo = jamInfo, EntriesData = entriesData };
        }

        private JamEntrySaveData? LoadEntryData(FilePath entriesPath, NewJamEntryStub stub)
        {
            var directoryPath = entriesPath.Append(stub.EntrySubpath);
            var entryInfoPath = directoryPath.Append("entry.jamentry");
            var entryInfo = EntryLoader.TryLoad(entryInfoPath);
            if (entryInfo == null)
                return null;

            return new JamEntrySaveData { DirectoryPath = directoryPath, EntryInfo = entryInfo };
        }
    }
}
