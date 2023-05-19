using System;
using System.IO;

namespace Alphicsh.JamTools.Common.IO.Jam.Serialization
{
    public class JamEntryInfoLoader
    {
        private JsonContentSerializer<JamEntryInfo> Serializer { get; }

        public JamEntryInfoLoader()
        {
            Serializer = new JsonContentSerializer<JamEntryInfo>();
        }

        public JamEntryInfo? TryLoadJamEntryInfo(FilePath filePath)
        {
            if (filePath.IsRelative())
                throw new ArgumentException("The jam entry info can only be read from the absolute file path.", nameof(filePath));

            if (!filePath.HasFile())
                return null;

            var content = File.ReadAllText(filePath.Value);
            return Serializer.Deserialize(content);
        }
    }
}
