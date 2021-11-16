using System.Collections.Generic;

using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPlayer.Model.Jam
{
    public class JamOverview
    {
        public FilePath DirectoryPath { get; init; } = default!;

        public IReadOnlyCollection<JamEntry> Entries { get; init; } = default!;
    }
}
