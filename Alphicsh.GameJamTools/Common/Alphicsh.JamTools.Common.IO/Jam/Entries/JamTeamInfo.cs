using System.Collections.Generic;

namespace Alphicsh.JamTools.Common.IO.Jam.Entries
{
    public class JamTeamInfo
    {
        public string? Name { get; init; }
        public IReadOnlyCollection<JamAuthorInfo> Authors { get; init; } = default!;
    }
}
