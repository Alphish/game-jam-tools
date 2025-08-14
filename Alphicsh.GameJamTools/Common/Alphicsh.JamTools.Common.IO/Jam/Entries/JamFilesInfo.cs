using System.Collections.Generic;

namespace Alphicsh.JamTools.Common.IO.Jam.Entries
{
    public class JamFilesInfo
    {
        public IReadOnlyCollection<JamLauncherInfo> Launchers { get; init; } = default!;
        public JamReadmeInfo? Readme { get; init; }
        public JamAfterwordInfo? Afterword { get; init; }
        public JamThumbnailsInfo? Thumbnails { get; init; }
    }
}
