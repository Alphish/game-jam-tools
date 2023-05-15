using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.EntryPackager.Model.Entry.Files
{
    public class JamThumbnailsEditable
    {
        public JamFilesEditable Files { get; }

        public string? ThumbnailLocation { get; set; }
        public bool HasThumbnailLocation
            => !string.IsNullOrWhiteSpace(ThumbnailLocation);
        public FilePath? ThumbnailFullLocation
            => HasThumbnailLocation ? Files.DirectoryPath.Append(ThumbnailLocation!) : null;

        public string? ThumbnailSmallLocation { get; set; }
        public bool HasThumbnailSmallLocation
            => !string.IsNullOrWhiteSpace(ThumbnailSmallLocation);
        public FilePath? ThumbnailSmallFullLocation
            => HasThumbnailSmallLocation ? Files.DirectoryPath.Append(ThumbnailSmallLocation!) : null;

        public JamThumbnailsEditable(JamFilesEditable files)
        {
            Files = files;
        }
    }
}
