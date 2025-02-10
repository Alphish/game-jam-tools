using System.Linq;
using Alphicsh.JamPlayer.Model.Jam.Files;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.IO.Jam.New;
using Alphicsh.JamTools.Common.IO.Jam.New.Entries;
using Alphicsh.JamTools.Common.IO.Storage;

namespace Alphicsh.JamPlayer.Model.Jam.Loading
{
    public class JamInfoToOverviewMapper : IMapper<NewJamInfo, JamOverview>
    {
        public JamOverview Map(NewJamInfo info)
        {
            var directoryPath = info.Location.GetParentDirectoryPath();
            var logoPath = directoryPath.AppendNullable(info.LogoFileName);
            var awardCriteria = info.AwardCriteria.Select(MapAwardCriterion).ToList();
            var entries = info.Entries.Select(MapEntry).ToList();

            return new JamOverview
            {
                DirectoryPath = directoryPath,
                Title = info.Title,
                Theme = info.Theme,
                LogoPath = logoPath,
                AwardCriteria = awardCriteria,
                Entries = entries,
            };
        }

        private JamAwardCriterion MapAwardCriterion(NewJamAwardInfo awardInfo)
        {
            return new JamAwardCriterion
            {
                Id = awardInfo.Id,
                Name = awardInfo.Name,
                Description = awardInfo.Description,
            };
        }

        private JamEntry MapEntry(NewJamEntryInfo entryInfo)
        {
            var entryDirectory = entryInfo.Location.GetParentDirectoryPath();
            return new JamEntry
            {
                Id = entryInfo.JamId!,
                Title = entryInfo.Title,
                ShortTitle = entryInfo.ShortTitle ?? entryInfo.Title,
                Team = MapTeam(entryInfo.Team),
                Files = MapFiles(entryInfo.Files, entryDirectory),
            };
        }

        // ----
        // Team
        // ----

        private JamTeam MapTeam(NewJamTeamInfo teamInfo)
        {
            var authors = teamInfo.Authors.Select(MapAuthor).ToList();
            return new JamTeam
            {
                Name = teamInfo.Name,
                Authors = authors,
            };
        }

        private JamAuthor MapAuthor(NewJamAuthorInfo authorInfo)
        {
            return new JamAuthor
            {
                Name = authorInfo.Name,
                Role = authorInfo.Role,
                CommunityId = authorInfo.CommunityId,
            };
        }

        // -----
        // Files
        // -----

        private JamFiles MapFiles(NewJamFilesInfo filesInfo, FilePath entryDirectory)
        {
            var launchers = filesInfo.Launchers
                .Select(launcherInfo => MapLauncher(launcherInfo, entryDirectory))
                .ToList();

            return new JamFiles
            {
                DirectoryPath = entryDirectory,
                Launchers = launchers,
                Thumbnails = filesInfo.Thumbnails != null ? MapThumbnails(filesInfo.Thumbnails, entryDirectory) : null,
                Readme = filesInfo.Readme != null ? MapReadme(filesInfo.Readme, entryDirectory) : null,
                Afterword = filesInfo.Afterword != null ? MapAfterword(filesInfo.Afterword, entryDirectory) : null,
            };
        }

        private LaunchData MapLauncher(NewJamLauncherInfo launcherInfo, FilePath entryDirectory)
        {
            var launchType = (LaunchType)launcherInfo.Type;
            var location = launchType == LaunchType.WindowsExe
                ? entryDirectory.Append(launcherInfo.Location).Value
                : launcherInfo.Location;
            return new LaunchData(launcherInfo.Name, launcherInfo.Description, launchType, location);
        }

        private JamThumbnails MapThumbnails(NewJamThumbnailsInfo thumbnailsInfo, FilePath entryDirectory)
        {
            return new JamThumbnails
            {
                ThumbnailPath = entryDirectory.AppendNullable(thumbnailsInfo.ThumbnailLocation),
                ThumbnailSmallPath = entryDirectory.AppendNullable(thumbnailsInfo.ThumbnailSmallLocation),
            };
        }

        private JamReadme MapReadme(NewJamReadmeInfo readmeInfo, FilePath entryDirectory)
        {
            return new JamReadme
            {
                Path = entryDirectory.Append(readmeInfo.Location),
                IsRequired = readmeInfo.IsRequired,
            };
        }

        private JamAfterword MapAfterword(NewJamAfterwordInfo afterwordInfo, FilePath entryDirectory)
        {
            return new JamAfterword
            {
                Path = entryDirectory.Append(afterwordInfo.Location),
            };
        }
    }
}
