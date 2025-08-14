using System.Collections.Generic;
using Alphicsh.EntryPackager.Model.Entry.Saving;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Jam.New;

namespace Alphicsh.JamPackager.Model.Jam.Saving
{
    public class JamSaveData
    {
        public FilePath DirectoryPath { get; init; }
        public NewJamCore JamInfo { get; init; } = default!;
        public IReadOnlyCollection<JamEntrySaveData> EntriesData { get; init; } = default!;

    }
}
