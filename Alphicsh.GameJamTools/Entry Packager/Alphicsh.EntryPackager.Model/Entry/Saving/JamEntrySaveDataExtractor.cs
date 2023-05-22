using System.Linq;
using Alphicsh.EntryPackager.Model.Entry.Files;
using Alphicsh.JamTools.Common.IO.Jam;
using Alphicsh.JamTools.Common.IO.Jam.Files;
using Alphicsh.JamTools.Common.IO.Saving;

namespace Alphicsh.EntryPackager.Model.Entry.Saving
{
    internal class JamEntrySaveDataExtractor : ISaveDataExtractor<JamEntryEditable, JamEntrySaveData>
    {
        public JamEntrySaveData ExtractData(JamEntryEditable model)
        {
            return new JamEntrySaveData
            {
                DirectoryPath = model.Files.DirectoryPath,
                EntryInfo = MapEntry(model),
            };
        }

        private JamEntryInfo MapEntry(JamEntryEditable entryEditable)
        {
            return new JamEntryInfo
            {
                Title = ToNullIfEmpty(entryEditable.Title) ?? string.Empty,
                ShortTitle = ToNullIfEmpty(entryEditable.ShortTitle),
                Team = MapTeam(entryEditable.Team),
                Files = MapFiles(entryEditable.Files),
            };
        }

        // ----
        // Team
        // ----

        private JamTeamInfo MapTeam(JamTeamEditable teamEditable)
        {
            return new JamTeamInfo
            {
                Name = ToNullIfEmpty(teamEditable.Name),
                Authors = teamEditable.Authors.Select(MapAuthor).ToList(),
            };
        }

        private JamAuthorInfo MapAuthor(JamAuthorEditable authorEditable)
        {
            return new JamAuthorInfo
            {
                Name = ToNullIfEmpty(authorEditable.Name) ?? string.Empty,
                CommunityId = ToNullIfEmpty(authorEditable.CommunityId),
                Role = ToNullIfEmpty(authorEditable.Role),
            };
        }

        // -----
        // Files
        // -----

        private JamFilesInfo MapFiles(JamFilesEditable filesEditable)
        {
            return new JamFilesInfo
            {
                Launchers = filesEditable.Launchers.Select(MapLauncher).ToList(),
                Readme = MapReadme(filesEditable.Readme),
                Afterword = MapAfterword(filesEditable.Afterword),
                Thumbnails = MapThumbnails(filesEditable.Thumbnails),
            };
        }

        private JamLauncherInfo MapLauncher(JamLauncherEditable launcherEditable)
        {
            return new JamLauncherInfo
            {
                Name = ToNullIfEmpty(launcherEditable.Name) ?? string.Empty,
                Description = ToNullIfEmpty(launcherEditable.Description),
                Type = (int)launcherEditable.Type,
                Location = ToNullIfEmpty(launcherEditable.Location) ?? string.Empty,
            };
        }

        private JamReadmeInfo? MapReadme(JamReadmeEditable readmeEditable)
        {
            if (readmeEditable.IsEmpty)
                return null;

            return new JamReadmeInfo { Location = readmeEditable.Location!, IsRequired = readmeEditable.IsRequired };
        }

        private JamAfterwordInfo? MapAfterword(JamAfterwordEditable afterwordEditable)
        {
            if (afterwordEditable.IsEmpty)
                return null;

            return new JamAfterwordInfo { Location = afterwordEditable.Location! };
        }

        private JamThumbnailsInfo? MapThumbnails(JamThumbnailsEditable thumbnailsEditable)
        {
            if (thumbnailsEditable.IsEmpty)
                return null;

            return new JamThumbnailsInfo
            {
                ThumbnailLocation = ToNullIfEmpty(thumbnailsEditable.ThumbnailLocation),
                ThumbnailSmallLocation = ToNullIfEmpty(thumbnailsEditable.ThumbnailSmallLocation),
            };
        }

        private string? ToNullIfEmpty(string? value)
        {
            return !string.IsNullOrWhiteSpace(value) ? value : null;
        }
    }
}
