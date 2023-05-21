using System;

namespace Alphicsh.JamTools.Common.IO.Jam.Files
{
    public class JamLauncherInfo
    {
        public string Name { get; init; } = default!;
        public string? Description { get; init; }
        public int Type { get; init; }
        public string Location { get; init; } = default!;

        // --------
        // Equality
        // --------

        public override bool Equals(object? obj)
        {
            return obj is JamLauncherInfo info &&
                   Name == info.Name &&
                   Description == info.Description &&
                   Type == info.Type &&
                   Location == info.Location;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Description, Type, Location);
        }
    }
}
