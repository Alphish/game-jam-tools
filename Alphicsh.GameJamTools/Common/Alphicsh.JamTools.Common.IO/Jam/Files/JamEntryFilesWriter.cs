using System.IO;

namespace Alphicsh.JamTools.Common.IO.Jam.Files
{
    public class JamEntryFilesWriter
    {
        private JsonContentSerializer<JamEntryInfo> JamEntryInfoSerializer { get; }

        public JamEntryFilesWriter()
        {
            JamEntryInfoSerializer = new JsonContentSerializer<JamEntryInfo>();
        }

        public void SaveJamEntryInfo(JamEntryInfo jamEntryInfo)
        {
            var content = JamEntryInfoSerializer.Serialize(jamEntryInfo);
            File.WriteAllText(jamEntryInfo.EntryInfoPath.Value, content);
        }
    }
}
