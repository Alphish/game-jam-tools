using System.Linq;
using Alphicsh.EntryPackager.Model.Entry.Files;
using Alphicsh.JamTools.Common.IO.Jam.New.Entries;
using Alphicsh.JamTools.Common.IO.Saving;

namespace Alphicsh.EntryPackager.Model.Entry.Saving
{
    public class JamEntrySaveDataExtractor : ISaveDataExtractor<JamEntryEditable, JamEntrySaveData>
    {
        private const string SavedVersion = "V2";

        private bool IsFromEntryPackager { get; set; }

        public JamEntrySaveDataExtractor(bool isFromEntryPackager)
        {
            IsFromEntryPackager = isFromEntryPackager;
        }

        public JamEntrySaveData ExtractData(JamEntryEditable model)
        {
            return new JamEntrySaveData
            {
                DirectoryPath = model.Files.DirectoryPath,
                EntryInfo = MapEntry(model),
            };
        }

        private NewJamEntryInfo MapEntry(JamEntryEditable entryEditable)
        {
            return new NewJamEntryInfo
            {
                Version = SavedVersion,
                WrittenBy = IsFromEntryPackager ? "EntryPackager" : (entryEditable.WrittenBy ?? "JamPackager"),
                Title = ToNullIfEmpty(entryEditable.Title) ?? string.Empty,
                ShortTitle = ToNullIfEmpty(entryEditable.ShortTitle),
                Alignment = ToNullIfEmpty(entryEditable.Alignment),
                Team = MapTeam(entryEditable.Team),
                Files = MapFiles(entryEditable.Files),
            };
        }

        // ----
        // Team
        // ----

        private NewJamTeamInfo MapTeam(JamTeamEditable teamEditable)
        {
            return new NewJamTeamInfo
            {
                Name = ToNullIfEmpty(teamEditable.Name),
                Authors = teamEditable.Authors.Select(MapAuthor).ToList(),
            };
        }

        private NewJamAuthorInfo MapAuthor(JamAuthorEditable authorEditable)
        {
            return new NewJamAuthorInfo
            {
                Name = ToNullIfEmpty(authorEditable.Name) ?? string.Empty,
                CommunityId = ToNullIfEmpty(authorEditable.CommunityId),
                Role = ToNullIfEmpty(authorEditable.Role),
            };
        }

        // -----
        // Files
        // -----

        private NewJamFilesInfo MapFiles(JamFilesEditable filesEditable)
        {
            return new NewJamFilesInfo
            {
                Launchers = filesEditable.Launchers.Select(MapLauncher).ToList(),
                Readme = MapReadme(filesEditable.Readme),
                Afterword = MapAfterword(filesEditable.Afterword),
                Thumbnails = MapThumbnails(filesEditable.Thumbnails),
            };
        }

        private NewJamLauncherInfo MapLauncher(JamLauncherEditable launcherEditable)
        {
            return new NewJamLauncherInfo
            {
                Name = ToNullIfEmpty(launcherEditable.Name) ?? string.Empty,
                Description = ToNullIfEmpty(launcherEditable.Description),
                Type = (int)launcherEditable.Type,
                Location = ToNullIfEmpty(launcherEditable.Location) ?? string.Empty,
            };
        }

        private NewJamReadmeInfo? MapReadme(JamReadmeEditable readmeEditable)
        {
            if (readmeEditable.IsEmpty)
                return null;

            return new NewJamReadmeInfo { Location = readmeEditable.Location!, IsRequired = readmeEditable.IsRequired };
        }

        private NewJamAfterwordInfo? MapAfterword(JamAfterwordEditable afterwordEditable)
        {
            if (afterwordEditable.IsEmpty)
                return null;

            return new NewJamAfterwordInfo { Location = afterwordEditable.Location! };
        }

        private NewJamThumbnailsInfo? MapThumbnails(JamThumbnailsEditable thumbnailsEditable)
        {
            if (thumbnailsEditable.IsEmpty)
                return null;

            return new NewJamThumbnailsInfo
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
