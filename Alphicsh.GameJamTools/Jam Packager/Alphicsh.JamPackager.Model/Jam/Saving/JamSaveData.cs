using System.Collections.Generic;
using Alphicsh.JamTools.Common.IO.Jam;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.EntryPackager.Model.Entry.Saving;

namespace Alphicsh.JamPackager.Model.Jam.Saving
{
    public class JamSaveData
    {
        public FilePath DirectoryPath { get; init; }
        public JamInfo JamInfo { get; init; } = default!;
        public IReadOnlyCollection<JamEntrySaveData> EntriesData { get; init; } = default!;

    }
}
