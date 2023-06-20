using System.Collections.Generic;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Execution;

namespace Alphicsh.JamPlayer.Model.Jam.Files
{
    public class JamFiles
    {
        public FilePath DirectoryPath { get; init; }
        public IReadOnlyCollection<LaunchData> Launchers { get; init; } = default!;
        public JamReadme? Readme { get; init; }
        public JamAfterword? Afterword { get; init; }
        public JamThumbnails? Thumbnails { get; init; }
    }
}
