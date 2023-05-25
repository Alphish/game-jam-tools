using System.IO;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.JamTools.Common.IO.Jam.Serialization
{
    public class JamEntryLegacyFilesWriter
    {
        private JsonContentSerializer<JamEntryLegacyInfo> JamEntryInfoSerializer { get; }

        public JamEntryLegacyFilesWriter()
        {
            JamEntryInfoSerializer = new JsonContentSerializer<JamEntryLegacyInfo>();
        }

        public void SaveJamEntryInfo(JamEntryLegacyInfo jamEntryInfo)
        {
            var content = JamEntryInfoSerializer.Serialize(jamEntryInfo);
            File.WriteAllText(jamEntryInfo.EntryInfoPath.Value, content);
        }
    }
}
