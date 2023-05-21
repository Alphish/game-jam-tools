using System;
using System.IO;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.JamTools.Common.IO.Jam.Serialization
{
    public class JamEntryLegacyFilesReader
    {
        private JsonContentSerializer<JamEntryLegacyInfo> JamEntryInfoSerializer { get; }

        public JamEntryLegacyFilesReader()
        {
            JamEntryInfoSerializer = new JsonContentSerializer<JamEntryLegacyInfo>();
        }

        public JamEntryLegacyInfo? TryLoadJamEntryInfo(string entryId, FilePath filePath)
        {
            if (filePath.IsRelative())
                throw new ArgumentException("The jam entry info can only be read from the absolute file path.", nameof(filePath));

            if (!filePath.HasFile())
                return null;

            var content = File.ReadAllText(filePath.Value);
            var jamEntryInfo = JamEntryInfoSerializer.Deserialize(content);
            if (jamEntryInfo == null)
                return null;

            jamEntryInfo.Id = entryId;
            jamEntryInfo.EntryInfoPath = filePath;

            return jamEntryInfo;
        }
    }
}
