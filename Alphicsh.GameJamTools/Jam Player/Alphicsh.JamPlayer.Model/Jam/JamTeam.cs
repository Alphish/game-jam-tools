using System.Collections.Generic;

namespace Alphicsh.JamPlayer.Model.Jam
{
    public sealed class JamTeam
    {
        public string? Name { get; init; } = null!;
        public IReadOnlyCollection<JamAuthor> Authors { get; init; } = null!;
    }
}
