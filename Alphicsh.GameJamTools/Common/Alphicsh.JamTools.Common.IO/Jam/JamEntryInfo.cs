using System.Collections.Generic;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.IO.Jam.Files;

namespace Alphicsh.JamTools.Common.IO.Jam
{
    public class JamEntryInfo
    {
        public string Title { get; init; } = default!;
        public JamTeamInfo Team { get; init; } = default!;
        public JamFilesInfo Files { get; init; } = default!;

        // -----------------
        // Legacy properties
        // -----------------

        public string? GameFileName { get; init; }
        public string? ThumbnailFileName { get; init; }
        public string? ThumbnailSmallFileName { get; init; }

        public string? ReadmeFileName { get; init; }
        public bool IsReadmePlease { get; init; }
        public string? AfterwordFileName { get; init; }

        public JamEntryInfo FromLegacyFormat()
        {
            if (Files != null)
                return this;

            var files = GetFilesFromLegacyFormat();
            return new JamEntryInfo { Title = Title, Team = Team, Files = files };
        }

        private JamFilesInfo GetFilesFromLegacyFormat()
        {
            var launchers = new List<JamLauncherInfo>();
            if (GameFileName != null)
            {
                var launcher = new JamLauncherInfo()
                {
                    Name = "Windows Executable",
                    Description = null,
                    Type = (int)LaunchType.WindowsExe,
                    Location = GameFileName
                };
                launchers.Add(launcher);
            }

            var readme = ReadmeFileName != null ? new JamReadmeInfo { Location = ReadmeFileName, IsRequired = IsReadmePlease } : null;
            var afterword = AfterwordFileName != null ? new JamAfterwordInfo { Location = AfterwordFileName } : null;
            var thumbnails = ThumbnailFileName != null || ThumbnailSmallFileName != null
                ? new JamThumbnailsInfo { ThumbnailLocation = ThumbnailFileName, ThumbnailSmallLocation = ThumbnailSmallFileName }
                : null;

            return new JamFilesInfo
            {
                Launchers = launchers,
                Readme = readme,
                Afterword = afterword,
                Thumbnails = thumbnails,
            };
        }
    }
}
