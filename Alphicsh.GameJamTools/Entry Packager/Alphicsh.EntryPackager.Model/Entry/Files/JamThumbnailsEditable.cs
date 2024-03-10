using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.EntryPackager.Model.Entry.Files
{
    public class JamThumbnailsEditable
    {
        public JamFilesEditable Files { get; }

        public FilePath? GetFullLocation(string? location)
            => !string.IsNullOrWhiteSpace(location) ? Files.DirectoryPath.Append(location) : null;

        public string? ThumbnailLocation { get; set; }
        public bool HasThumbnailLocation => !string.IsNullOrWhiteSpace(ThumbnailLocation);
        public FilePath? ThumbnailFullLocation => GetFullLocation(ThumbnailLocation);

        public string? ThumbnailSmallLocation { get; set; }
        public bool HasThumbnailSmallLocation => !string.IsNullOrWhiteSpace(ThumbnailSmallLocation);
        public FilePath? ThumbnailSmallFullLocation => GetFullLocation(ThumbnailSmallLocation);

        public bool IsEmpty => string.IsNullOrWhiteSpace(ThumbnailLocation) && string.IsNullOrWhiteSpace(ThumbnailSmallLocation);

        public JamThumbnailsEditable(JamFilesEditable files)
        {
            Files = files;
        }

        internal void ClearInvalid()
        {
            if (!IsValidThumbnail(ThumbnailFullLocation))
                ThumbnailLocation = null;

            if (!IsValidThumbnail(ThumbnailSmallFullLocation))
                ThumbnailSmallLocation = null;
        }

        private bool IsValidThumbnail(FilePath? location)
        {
            if (location == null)
                return false;

            if (!location.Value.HasFile())
                return false;

            return true;
        }
    }
}
