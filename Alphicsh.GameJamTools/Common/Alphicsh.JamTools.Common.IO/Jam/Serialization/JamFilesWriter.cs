using System.IO;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.JamTools.Common.IO.Jam.Serialization
{
    public class JamFilesWriter
    {
        private JsonContentSerializer<JamInfo> JamInfoSerializer { get; }
        private JamEntryLegacyFilesWriter EntryFilesWriter { get; }

        public JamFilesWriter()
        {
            JamInfoSerializer = new JsonContentSerializer<JamInfo>();
            EntryFilesWriter = new JamEntryLegacyFilesWriter();
        }

        public void SaveJamInfo(JamInfo jamInfo)
        {
            var content = JamInfoSerializer.Serialize(jamInfo);
            File.WriteAllText(jamInfo.JamInfoPath.Value, content);

            foreach (var entry in jamInfo.Entries)
            {
                EntryFilesWriter.SaveJamEntryInfo(entry);
            }
        }
    }
}
