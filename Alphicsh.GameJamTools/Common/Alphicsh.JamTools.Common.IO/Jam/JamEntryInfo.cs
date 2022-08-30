using System.Text.Json.Serialization;

namespace Alphicsh.JamTools.Common.IO.Jam
{
    public class JamEntryInfo
    {
        [JsonIgnore] public string Id { get; internal set; } = default!;

        // ----------------
        // Basic properties
        // ----------------

        public string Title { get; init; } = default!;
        public JamTeamInfo Team { get; init; } = default!;

        public string? GameFileName { get; set; }
        public string? ThumbnailFileName { get; set; }
        public string? ThumbnailSmallFileName { get; set; }

        public string? ReadmeFileName { get; set; }
        public bool IsReadmePlease { get; set; }
        public string? AfterwordFileName { get; set; }

        // ---------------------
        // Filesystem properties
        // ---------------------

        [JsonIgnore] public FilePath EntryInfoPath { get; set; }

        [JsonIgnore] public FilePath EntryDirectoryPath
        {
            get => EntryInfoPath.GetParentDirectoryPath()!.Value;
            set => EntryInfoPath = value.Append(EntryInfoFileName);
        }

        [JsonIgnore] public string EntryInfoFileName
        {
            get => EntryInfoPath.GetLastSegmentName();
            set => EntryInfoPath = EntryDirectoryPath.Append(value);
        }

        [JsonIgnore] public FilePath? GamePath
            => EntryDirectoryPath.AppendNullable(GameFileName);
        [JsonIgnore] public FilePath? ThumbnailPath
            => EntryDirectoryPath.AppendNullable(ThumbnailFileName);
        [JsonIgnore] public FilePath? ThumbnailSmallPath
            => EntryDirectoryPath.AppendNullable(ThumbnailSmallFileName);

        [JsonIgnore] public FilePath? ReadmePath
            => EntryDirectoryPath.AppendNullable(ReadmeFileName);
        [JsonIgnore] public FilePath? AfterwordPath
            => EntryDirectoryPath.AppendNullable(AfterwordFileName);
    }
}
