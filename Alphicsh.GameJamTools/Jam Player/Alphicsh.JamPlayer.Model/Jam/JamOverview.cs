using System.Collections.Generic;

namespace Alphicsh.JamPlayer.Model.Jam
{
    public class JamOverview
    {
        public IReadOnlyCollection<JamEntry> Entries { get; init; } = default!;
    }
}
