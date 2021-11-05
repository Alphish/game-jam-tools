using System.IO;

namespace Alphicsh.JamTools.Common.IO.Jam.Files
{
    public class JamFilesWriter
    {
        private JsonContentSerializer<JamInfo> JamInfoSerializer { get; }
        private JamEntryFilesWriter EntryFilesWriter { get; }

        public JamFilesWriter()
        {
            JamInfoSerializer = new JsonContentSerializer<JamInfo>();
            EntryFilesWriter = new JamEntryFilesWriter();
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
