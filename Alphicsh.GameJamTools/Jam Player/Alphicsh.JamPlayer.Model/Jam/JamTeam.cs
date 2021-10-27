using System.Collections.Generic;

namespace Alphicsh.JamPlayer.Model.Jam
{
    public sealed class JamTeam
    {
        public string? Name { get; init; } = default!;
        public IReadOnlyCollection<JamAuthor> Authors { get; init; } = default!;
    }
}
