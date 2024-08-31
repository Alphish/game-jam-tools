using System.Collections.Generic;

namespace Alphicsh.JamTools.Common.IO.Jam.New.Entries
{
    public class NewJamTeamInfo
    {
        public string? Name { get; init; }
        public IReadOnlyCollection<NewJamAuthorInfo> Authors { get; init; } = default!;
    }
}
