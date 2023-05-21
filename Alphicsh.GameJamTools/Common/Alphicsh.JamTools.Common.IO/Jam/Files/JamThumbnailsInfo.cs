using System;

namespace Alphicsh.JamTools.Common.IO.Jam.Files
{
    public class JamThumbnailsInfo
    {
        public string? ThumbnailLocation { get; init; } = default!;
        public string? ThumbnailSmallLocation { get; init; } = default!;

        // --------
        // Equality
        // --------

        public override bool Equals(object? obj)
        {
            return obj is JamThumbnailsInfo info &&
                   ThumbnailLocation == info.ThumbnailLocation &&
                   ThumbnailSmallLocation == info.ThumbnailSmallLocation;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ThumbnailLocation, ThumbnailSmallLocation);
        }
    }
}
