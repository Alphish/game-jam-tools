using System.Linq;
using Alphicsh.JamPlayer.Model.Jam.Files;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.IO.Jam;
using Alphicsh.JamTools.Common.IO.Jam.Files;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.JamPlayer.Model.Jam.Loading
{
    public class JamEntryLoader
    {
        private static JsonFileLoader<JamEntryInfo> Loader { get; } = new JsonFileLoader<JamEntryInfo>();

        public JamEntry? ReadFromDirectory(string id, FilePath directoryPath)
        {
            var entryInfoPath = directoryPath.Append("entry.jamentry");
            var entryInfo = Loader.TryLoad(entryInfoPath)?.FromLegacyFormat();
            return entryInfo != null ? MapEntry(id, directoryPath, entryInfo) : null;
        }

        // -------
        // Mapping
        // -------

        private JamEntry MapEntry(string id, FilePath directoryPath, JamEntryInfo entryInfo)
        {
            return new JamEntry
            {
                Id = id,
                Title = entryInfo.Title,
                ShortTitle = entryInfo.ShortTitle ?? entryInfo.Title,
                Team = MapTeam(entryInfo.Team),
                Files = MapFiles(directoryPath, entryInfo.Files),
            };
        }

        // Team

        private JamTeam MapTeam(JamTeamInfo teamInfo)
        {
            return new JamTeam
            {
                Name = teamInfo.Name,
                Authors = teamInfo.Authors.Select(MapAuthor).ToList(),
            };
        }

        private JamAuthor MapAuthor(JamAuthorInfo authorInfo)
        {
            return new JamAuthor
            {
                Name = authorInfo.Name,
                CommunityId = authorInfo.CommunityId,
                Role = authorInfo.Role,
            };
        }

        // Files

        private JamFiles MapFiles(FilePath directoryPath, JamFilesInfo filesInfo)
        {
            return new JamFiles
            {
                DirectoryPath = directoryPath,
                Launchers = filesInfo.Launchers.Select(launcher => MapLauncher(directoryPath, launcher)).ToList(),
                Readme = filesInfo.Readme != null ? MapReadme(directoryPath, filesInfo.Readme) : null,
                Afterword = filesInfo.Afterword != null ? MapAfterword(directoryPath, filesInfo.Afterword) : null,
                Thumbnails = filesInfo.Thumbnails != null ? MapThumbnails(directoryPath, filesInfo.Thumbnails) : null,
            };
        }

        private LaunchData MapLauncher(FilePath directoryPath, JamLauncherInfo launcherInfo)
        {
            var launchType = (LaunchType)launcherInfo.Type;
            var location = launchType == LaunchType.WindowsExe
                ? directoryPath.Append(launcherInfo.Location).Value
                : launcherInfo.Location;
            return new LaunchData(launcherInfo.Name, launcherInfo.Description, launchType, location);
        }

        private JamReadme MapReadme(FilePath directoryPath, JamReadmeInfo readmeInfo)
        {
            return new JamReadme
            {
                Path = directoryPath.Append(readmeInfo.Location),
                IsRequired = readmeInfo.IsRequired,
            };
        }

        private JamAfterword MapAfterword(FilePath directoryPath, JamAfterwordInfo afterwordInfo)
        {
            return new JamAfterword { Path = directoryPath.Append(afterwordInfo.Location) };
        }

        private JamThumbnails MapThumbnails(FilePath directoryPath, JamThumbnailsInfo thumbnails)
        {
            return new JamThumbnails
            {
                ThumbnailPath = directoryPath.AppendNullable(thumbnails.ThumbnailLocation),
                ThumbnailSmallPath = directoryPath.AppendNullable(thumbnails.ThumbnailSmallLocation),
            };
        }
    }
}
