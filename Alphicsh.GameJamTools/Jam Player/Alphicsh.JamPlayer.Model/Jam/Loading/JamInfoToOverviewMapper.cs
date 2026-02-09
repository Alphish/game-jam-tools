using System;
using System.Linq;
using Alphicsh.JamPlayer.Model.Jam.Files;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.IO.Jam;
using Alphicsh.JamTools.Common.IO.Jam.Entries;
using Alphicsh.JamTools.Common.IO.Storage;

namespace Alphicsh.JamPlayer.Model.Jam.Loading
{
    public class JamInfoToOverviewMapper : IMapper<JamInfo, JamOverview>
    {
        public JamOverview Map(JamInfo info)
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
                AttributionSpecified = entries.Any(entry => entry.Attribution.AnySpecified),
            };
        }

        private JamAwardCriterion MapAwardCriterion(JamAwardInfo awardInfo)
        {
            return new JamAwardCriterion
            {
                Id = awardInfo.Id,
                Name = awardInfo.Name,
                Description = awardInfo.Description,
            };
        }

        private JamEntry MapEntry(JamEntryInfo entryInfo)
        {
            var entryDirectory = entryInfo.Location.GetParentDirectoryPath();
            return new JamEntry
            {
                Id = entryInfo.JamId!,
                Title = entryInfo.Title,
                ShortTitle = entryInfo.ShortTitle ?? entryInfo.Title,
                Team = MapTeam(entryInfo.Team),
                Attribution = MapAttribution(entryInfo.Attribution),
                Files = MapFiles(entryInfo.Files, entryDirectory),
                PageUrl = entryInfo.PageUrl,
            };
        }

        // ----
        // Team
        // ----

        private JamTeam MapTeam(JamTeamInfo teamInfo)
        {
            var authors = teamInfo.Authors.Select(MapAuthor).ToList();
            return new JamTeam
            {
                Name = teamInfo.Name,
                Authors = authors,
            };
        }

        private JamAuthor MapAuthor(JamAuthorInfo authorInfo)
        {
            return new JamAuthor
            {
                Name = authorInfo.Name,
                Role = authorInfo.Role,
                CommunityId = authorInfo.CommunityId,
            };
        }

        // -----------
        // Attribution
        // -----------

        private JamAttribution MapAttribution(JamAttributionInfo? attributionInfo)
        {
            if (attributionInfo == null)
            {
                return new JamAttribution
                {
                    Code = ComponentAttribution.Unspecified,
                    Visuals = ComponentAttribution.Unspecified,
                    Music = ComponentAttribution.Unspecified,
                    Sounds = ComponentAttribution.Unspecified,
                };
            }

            return new JamAttribution
            {
                Code = Enum.TryParse<ComponentAttribution>(attributionInfo.Code, ignoreCase: true, out var code) ? code : ComponentAttribution.Unspecified,
                Visuals = Enum.TryParse<ComponentAttribution>(attributionInfo.Visuals, ignoreCase: true, out var visuals) ? visuals : ComponentAttribution.Unspecified,
                Music = Enum.TryParse<ComponentAttribution>(attributionInfo.Music, ignoreCase: true, out var music) ? music : ComponentAttribution.Unspecified,
                Sounds = Enum.TryParse<ComponentAttribution>(attributionInfo.Sounds, ignoreCase: true, out var sounds) ? sounds : ComponentAttribution.Unspecified,
            };
        }

        // -----
        // Files
        // -----

        private JamFiles MapFiles(JamFilesInfo filesInfo, FilePath entryDirectory)
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

        private LaunchData MapLauncher(JamLauncherInfo launcherInfo, FilePath entryDirectory)
        {
            var launchType = (LaunchType)launcherInfo.Type;
            var location = launchType == LaunchType.WindowsExe
                ? entryDirectory.Append(launcherInfo.Location).Value
                : launcherInfo.Location;
            return new LaunchData(launcherInfo.Name, launcherInfo.Description, launchType, location);
        }

        private JamThumbnails MapThumbnails(JamThumbnailsInfo thumbnailsInfo, FilePath entryDirectory)
        {
            return new JamThumbnails
            {
                ThumbnailPath = entryDirectory.AppendNullable(thumbnailsInfo.ThumbnailLocation),
                ThumbnailSmallPath = entryDirectory.AppendNullable(thumbnailsInfo.ThumbnailSmallLocation),
            };
        }

        private JamReadme MapReadme(JamReadmeInfo readmeInfo, FilePath entryDirectory)
        {
            return new JamReadme
            {
                Path = entryDirectory.Append(readmeInfo.Location),
                IsRequired = readmeInfo.IsRequired,
            };
        }

        private JamAfterword MapAfterword(JamAfterwordInfo afterwordInfo, FilePath entryDirectory)
        {
            return new JamAfterword
            {
                Path = entryDirectory.Append(afterwordInfo.Location),
            };
        }
    }
}
