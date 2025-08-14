using System;
using System.Collections.Generic;
using Alphicsh.JamTools.Common.IO.Jam.Entries;
using Alphicsh.JamTools.Common.IO.Storage;
using Alphicsh.JamTools.Common.IO.Storage.Formats;
using Alphicsh.JamTools.Common.IO.Storage.Loading;

namespace Alphicsh.JamTools.Common.IO.Jam.Loading
{
    public class JamInfoReader : BaseModelInfoReader<JamInfo, JamCore>
    {
        private static JsonFormatter<JamCore> JamCoreFormatter { get; } = new JsonFormatter<JamCore>();
        private static JsonFormatter<JamEntryInfo> JamEntryFormatter { get; } = new JsonFormatter<JamEntryInfo>();

        protected override FilePath? LocateCore(FilePath dataLocation)
        {
            if (dataLocation.HasDirectory())
                dataLocation = dataLocation.Append("jam.jaminfo");

            if (dataLocation.HasFileOrUpdate() && dataLocation.GetExtension().Equals(".jaminfo", StringComparison.OrdinalIgnoreCase))
                return dataLocation;

            return null;
        }

        protected override JamCore? DeserializeCore(FileData coreFile)
        {
            var core = JamCoreFormatter.ParseFromFile(coreFile);
            if (core != null)
                core.Location = coreFile.Path;

            return core;
        }

        protected override IEnumerable<FilePath> LocateAuxiliaryFiles(JamCore coreData)
        {
            var entriesPath = coreData.Directory.Append(coreData.EntriesSubpath);
            foreach (var stub in coreData.Entries)
            {
                yield return entriesPath.Append(stub.EntrySubpath).Append("entry.jamentry");
            }
        }

        protected override JamInfo DeserializeModelInfo(FileBatch fileBatch, JamCore coreData)
        {
            List<JamEntryInfo> entries = new List<JamEntryInfo>();

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

            return new JamInfo(coreData, entries);
        }
    }
}
