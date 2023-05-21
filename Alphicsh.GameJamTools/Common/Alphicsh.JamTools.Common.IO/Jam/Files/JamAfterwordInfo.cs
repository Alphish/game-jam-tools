using System;

namespace Alphicsh.JamTools.Common.IO.Jam.Files
{
    public class JamAfterwordInfo
    {
        public string Location { get; init; } = default!;

        // --------
        // Equality
        // --------

        public override bool Equals(object? obj)
        {
            return obj is JamAfterwordInfo info &&
                   Location == info.Location;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Location);
        }
    }
}
