using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamTools.Common.IO.Jam.Files
{
    public class JamFilesInfo
    {
        public IReadOnlyCollection<JamLauncherInfo> Launchers { get; init; } = default!;
        public JamReadmeInfo? Readme { get; init; }
        public JamAfterwordInfo? Afterword { get; init; }
        public JamThumbnailsInfo? Thumbnails { get; init; }

        // --------
        // Equality
        // --------

        public override bool Equals(object? obj)
        {
            return obj is JamFilesInfo info &&
                   Launchers.SequenceEqual(info.Launchers) &&
                   EqualityComparer<JamReadmeInfo?>.Default.Equals(Readme, info.Readme) &&
                   EqualityComparer<JamAfterwordInfo?>.Default.Equals(Afterword, info.Afterword) &&
                   EqualityComparer<JamThumbnailsInfo?>.Default.Equals(Thumbnails, info.Thumbnails);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            foreach (var launcher in Launchers)
            {
                hash.Add(launcher);
            }
            hash.Add(Readme);
            hash.Add(Afterword);
            hash.Add(Thumbnails);
            return hash.ToHashCode();
        }
    }
}
