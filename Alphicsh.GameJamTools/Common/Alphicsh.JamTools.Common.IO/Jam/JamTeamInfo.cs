using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamTools.Common.IO.Jam
{
    public class JamTeamInfo
    {
        public string? Name { get; init; }
        public IReadOnlyCollection<JamAuthorInfo> Authors { get; init; } = default!;

        // --------
        // Equality
        // --------

        public override bool Equals(object? obj)
        {
            return obj is JamTeamInfo info &&
                   Name == info.Name &&
                   Authors.SequenceEqual(info.Authors);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Name);
            foreach (var author in Authors)
            {
                hash.Add(author);
            }
            return hash.ToHashCode();
        }
    }
}
