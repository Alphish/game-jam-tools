using Alphicsh.EntryPackager.Model.Entry.Saving;
using Alphicsh.JamTools.Common.IO.Jam;
using Alphicsh.JamTools.Common.IO.Saving;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.JamPackager.Model.Jam.Saving
{
    public class JamDataSaver : IDataSaver<JamSaveData>
    {
        private static JsonFileSaver<JamCore> Saver { get; } = new JsonFileSaver<JamCore>();
        private static JamEntryDataSaver EntrySaver { get; } = new JamEntryDataSaver();

        public void Save(JamSaveData saveData)
        {
            var jamInfoPath = saveData.DirectoryPath.Append("jam.jaminfo");
            Saver.Save(jamInfoPath, saveData.JamInfo);

            foreach (var entryData in saveData.EntriesData)
            {
                EntrySaver.Save(entryData);
            }
        }
    }
}
