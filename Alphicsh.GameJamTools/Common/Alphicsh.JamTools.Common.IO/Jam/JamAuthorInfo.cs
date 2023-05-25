using System;

namespace Alphicsh.JamTools.Common.IO.Jam
{
    public class JamAuthorInfo
    {
        public string Name { get; init; } = default!;
        public string? CommunityId { get; init; }
        public string? Role { get; init; }

        // --------
        // Equality
        // --------

        public override bool Equals(object? obj)
        {
            return obj is JamAuthorInfo info &&
                   Name == info.Name &&
                   CommunityId == info.CommunityId &&
                   Role == info.Role;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, CommunityId, Role);
        }
    }
}
