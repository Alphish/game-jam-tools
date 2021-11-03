using System;
using System.IO;

namespace Alphicsh.JamTools.Common.IO.Jam.Files
{
    public class JamEntryFilesReader
    {
        private JsonContentSerializer<JamEntryInfo> JamEntryInfoSerializer { get; }

        public JamEntryFilesReader()
        {
            JamEntryInfoSerializer = new JsonContentSerializer<JamEntryInfo>();
        }

        public JamEntryInfo? TryLoadJamEntryInfo(string entryId, FilePath filePath)
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
            jamEntryInfo.EntryDirectoryPath = filePath.GetParentDirectoryPath()!.Value;

            return jamEntryInfo;
        }
    }
}
