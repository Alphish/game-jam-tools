using System.Collections.Generic;

namespace Alphicsh.JamTools.Common.IO.Jam.New.Entries
{
    public class NewJamFilesInfo
    {
        public IReadOnlyCollection<NewJamLauncherInfo> Launchers { get; init; } = default!;
        public NewJamReadmeInfo? Readme { get; init; }
        public NewJamAfterwordInfo? Afterword { get; init; }
        public NewJamThumbnailsInfo? Thumbnails { get; init; }
    }
}
