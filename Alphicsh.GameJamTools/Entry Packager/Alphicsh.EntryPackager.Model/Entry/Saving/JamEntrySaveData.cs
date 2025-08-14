using System;
using System.Collections.Generic;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Jam.Entries;

namespace Alphicsh.EntryPackager.Model.Entry.Saving
{
    public class JamEntrySaveData
    {
        public FilePath DirectoryPath { get; init; }
        public JamEntryInfo EntryInfo { get; init; } = default!;

        // --------
        // Equality
        // --------

        public override bool Equals(object? obj)
        {
            return obj is JamEntrySaveData data &&
                   DirectoryPath.Equals(data.DirectoryPath) &&
                   EqualityComparer<JamEntryInfo>.Default.Equals(EntryInfo, data.EntryInfo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DirectoryPath, EntryInfo);
        }
    }
}
