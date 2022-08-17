using System.Collections.Generic;

namespace Alphicsh.JamPlayer.Model.Awards
{
    public class AwardsOverview
    {
        public IReadOnlyCollection<AwardEntry> Entries { get; init; } = default!;
    }
}
