using System;
using System.IO;

namespace Alphicsh.JamTools.Common.IO.Jam.Files
{
    public class JamFilesReader
    {
        private JsonContentSerializer<JamInfo> JamInfoSerializer { get; }

        public JamFilesReader()
        {
            JamInfoSerializer = new JsonContentSerializer<JamInfo>();
        }

        public JamInfo? TryLoadJamInfo(FilePath filePath)
        {
            if (filePath.IsRelative())
                throw new ArgumentException("The jam info can only be read from the absolute file path.", nameof(filePath));

            var content = File.ReadAllText(filePath.Value);
            var jamInfo = JamInfoSerializer.Deserialize(content);
            if (jamInfo == null)
                return null;

            jamInfo.JamInfoPath = filePath;
            jamInfo.JamDirectoryPath = filePath.GetParentDirectoryPath()!.Value;

            return jamInfo;
        }
    }
}
