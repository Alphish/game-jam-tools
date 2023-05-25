using System;

namespace Alphicsh.JamTools.Common.IO.Jam.Files
{
    public class JamReadmeInfo
    {
        public string Location { get; init; } = default!;
        public bool IsRequired { get; init; }

        // --------
        // Equality
        // --------

        public override bool Equals(object? obj)
        {
            return obj is JamReadmeInfo info &&
                   Location == info.Location &&
                   IsRequired == info.IsRequired;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Location, IsRequired);
        }
    }
}
