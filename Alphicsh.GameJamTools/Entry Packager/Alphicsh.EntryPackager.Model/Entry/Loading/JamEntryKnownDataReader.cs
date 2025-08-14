using System.Linq;
using Alphicsh.EntryPackager.Model.Entry.Files;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.IO.Jam.Entries;
using Alphicsh.JamTools.Common.IO.Search;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.EntryPackager.Model.Entry.Loading
{
    internal class JamEntryKnownDataReader
    {
        private static JsonFileLoader<JamEntryInfo> Loader { get; } = new JsonFileLoader<JamEntryInfo>();

        public JamEntryEditable? TryReadFromDirectory(FilePath directoryPath)
        {
            var entryInfo = FindEntryInfoForDirectory(directoryPath);
            return entryInfo != null ? CreateEntryForInfo(directoryPath, entryInfo) : null;
        }

        public JamEntryEditable? TryReadFromFile(FilePath entryInfoPath)
        {
            var entryInfo = ReadEntryInfo(entryInfoPath);
            return entryInfo != null ? CreateEntryForInfo(entryInfoPath.GetParentDirectoryPath(), entryInfo) : null;
        }

        // ------------
        // Finding info
        // ------------

        private JamEntryInfo? FindEntryInfoForDirectory(FilePath directoryPath)
        {
            var entryInfoPath = FindEntryInfoPath(directoryPath);
            return entryInfoPath != null ? ReadEntryInfo(entryInfoPath.Value) : null;
        }

        private FilePath? FindEntryInfoPath(FilePath directoryPath)
        {
            return FilesystemSearch.ForFilesIn(directoryPath)
                .IncludingTopDirectoryOnly()
                .FindMatches("entry.jamentry")
                .FirstOrDefault();
        }

        private JamEntryInfo? ReadEntryInfo(FilePath entryInfoPath)
        {
            return Loader.TryLoad(entryInfoPath)?.FromLegacyFormat();
        }

        // ----------------
        // Populating entry
        // ----------------

        public JamEntryEditable CreateEntryForInfo(FilePath directoryPath, JamEntryInfo entryInfo)
        {
            var entryEditable = new JamEntryEditable();
            entryEditable.Files.SetDirectoryPath(directoryPath);
            ApplyEntryInfo(entryEditable, entryInfo);
            return entryEditable;
        }

        private void ApplyEntryInfo(JamEntryEditable entryEditable, JamEntryInfo entryInfo)
        {
            entryEditable.WrittenBy = entryInfo.WrittenBy;
            entryEditable.Title = entryInfo.Title;
            entryEditable.ShortTitle = entryInfo.ShortTitle;
            entryEditable.Alignment = entryInfo.Alignment;
            ApplyTeamInfo(entryEditable.Team, entryInfo.Team);
            ApplyFilesInfo(entryEditable.Files, entryInfo.Files);
        }

        private void ApplyTeamInfo(JamTeamEditable teamEditable, JamTeamInfo teamInfo)
        {
            teamEditable.Name = teamInfo.Name;

            var authors = teamInfo.Authors.Select(info => new JamAuthorEditable
            {
                Name = info.Name,
                CommunityId = info.CommunityId,
                Role = info.Role,
            });

            foreach (var author in authors)
            {
                teamEditable.Authors.Add(author);
            }
        }

        private void ApplyFilesInfo(JamFilesEditable filesEditable, JamFilesInfo filesInfo)
        {
            foreach (var launcher in filesInfo.Launchers)
            {
                ApplyLauncher(filesEditable, launcher);
            }

            filesEditable.Readme.Location = filesInfo.Readme?.Location;
            filesEditable.Readme.IsRequired = filesInfo.Readme?.IsRequired ?? false;

            filesEditable.Afterword.Location = filesInfo.Afterword?.Location;

            filesEditable.Thumbnails.ThumbnailLocation = filesInfo.Thumbnails?.ThumbnailLocation;
            filesEditable.Thumbnails.ThumbnailSmallLocation = filesInfo.Thumbnails?.ThumbnailSmallLocation;
        }

        private void ApplyLauncher(JamFilesEditable files, JamLauncherInfo launcherInfo)
        {
            var launcherEditable = files.AddLauncher();
            launcherEditable.SetName(launcherInfo.Name);
            launcherEditable.SetDescription(launcherInfo.Description);
            launcherEditable.SetType((LaunchType)launcherInfo.Type);
            launcherEditable.SetLocation(launcherInfo.Location);
        }
    }
}
