using System;
using System.Collections.Generic;
using Alphicsh.JamTools.Common.IO.Jam.New.Entries;
using Alphicsh.JamTools.Common.IO.Storage;
using Alphicsh.JamTools.Common.IO.Storage.Formats;
using Alphicsh.JamTools.Common.IO.Storage.Loading;

namespace Alphicsh.JamTools.Common.IO.Jam.New.Loading
{
    public class JamInfoReader : IModelInfoReader<NewJamInfo, NewJamCore>
    {
        private static JsonFormatter<NewJamCore> JamCoreFormatter { get; } = new JsonFormatter<NewJamCore>();
        private static JsonFormatter<NewJamEntryInfo> JamEntryFormatter { get; } = new JsonFormatter<NewJamEntryInfo>();

        public FilePath? LocateCore(FilePath dataLocation)
        {
            if (dataLocation.HasDirectory())
                dataLocation = dataLocation.Append("jam.jaminfo");

            if (dataLocation.HasFile() && dataLocation.GetExtension().Equals(".jaminfo", StringComparison.OrdinalIgnoreCase))
                return dataLocation;

            return null;
        }

        public NewJamCore? DeserializeCore(FileData coreFile)
        {
            var core = JamCoreFormatter.ParseFromFile(coreFile);
            if (core != null)
                core.Location = coreFile.Path;

            return core;
        }

        public IEnumerable<FilePath> LocateAuxiliaryFiles(NewJamCore coreData)
        {
            var entriesPath = coreData.Directory.Append(coreData.EntriesSubpath);
            foreach (var stub in coreData.Entries)
            {
                yield return entriesPath.Append(stub.EntrySubpath).Append("entry.jamentry");
            }
        }

        public NewJamInfo? DeserializeModelInfo(FileBatch fileBatch, NewJamCore coreData)
        {
            List<NewJamEntryInfo> entries = new List<NewJamEntryInfo>();

            var entriesPath = coreData.Directory.Append(coreData.EntriesSubpath);
            foreach (var stub in coreData.Entries)
            {
                var entryLocation = entriesPath.Append(stub.EntrySubpath).Append("entry.jamentry");
                var entryFile = fileBatch.GetFileAt(entryLocation);
                if (entryFile == null)
                    continue;

                var entry = JamEntryFormatter.ParseFromFile(entryFile)?.FromLegacyFormat();
                if (entry == null)
                    continue;

                entry.Location = entryFile.Path;
                entry.JamId = stub.Id;
                entries.Add(entry);
            }

            return new NewJamInfo(coreData, entries);
        }
    }
}
