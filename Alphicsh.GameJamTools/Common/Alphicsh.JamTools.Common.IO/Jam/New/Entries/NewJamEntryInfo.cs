using System.Collections.Generic;
using System.Text.Json.Serialization;
using Alphicsh.JamTools.Common.IO.Execution;

namespace Alphicsh.JamTools.Common.IO.Jam.New.Entries
{
    public class NewJamEntryInfo
    {
        [JsonIgnore] public FilePath Location { get; internal set; }

        public string Version { get; init; } = default!;
        public string WrittenBy { get; init; } = default!;

        public string? JamId { get; set; }

        public string Title { get; init; } = default!;
        public string? ShortTitle { get; init; }
        public string? Alignment { get; init; }
        public NewJamTeamInfo Team { get; init; } = default!;
        public NewJamFilesInfo Files { get; init; } = default!;

        // -----------------
        // Legacy properties
        // -----------------

        public string? GameFileName { get; init; }
        public string? ThumbnailFileName { get; init; }
        public string? ThumbnailSmallFileName { get; init; }

        public string? ReadmeFileName { get; init; }
        public bool? IsReadmePlease { get; init; }
        public string? AfterwordFileName { get; init; }

        public NewJamEntryInfo FromLegacyFormat()
        {
            if (Files != null)
                return this;

            var files = GetFilesFromLegacyFormat();
            return new NewJamEntryInfo { Title = Title, ShortTitle = ShortTitle, Team = Team, Files = files };
        }

        private NewJamFilesInfo GetFilesFromLegacyFormat()
        {
            var launchers = new List<NewJamLauncherInfo>();
            if (GameFileName != null)
            {
                var launcher = new NewJamLauncherInfo()
                {
                    Name = "Windows Executable",
                    Description = null,
                    Type = (int)LaunchType.WindowsExe,
                    Location = GameFileName
                };
                launchers.Add(launcher);
            }

            var readme = ReadmeFileName != null ? new NewJamReadmeInfo
            {
                Location = ReadmeFileName,
                IsRequired = IsReadmePlease ?? false,
            } : null;

            var afterword = AfterwordFileName != null ? new NewJamAfterwordInfo { Location = AfterwordFileName } : null;
            var thumbnails = ThumbnailFileName != null || ThumbnailSmallFileName != null
                ? new NewJamThumbnailsInfo { ThumbnailLocation = ThumbnailFileName, ThumbnailSmallLocation = ThumbnailSmallFileName }
                : null;

            return new NewJamFilesInfo
            {
                Launchers = launchers,
                Readme = readme,
                Afterword = afterword,
                Thumbnails = thumbnails,
            };
        }
    }
}
