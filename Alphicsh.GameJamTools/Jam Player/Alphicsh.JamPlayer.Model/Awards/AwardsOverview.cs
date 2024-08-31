using System.Collections.Generic;

namespace Alphicsh.JamPlayer.Model.Awards
{
    public class AwardsOverview
    {
        public IReadOnlyCollection<AwardEntry> Entries { get; init; } = new List<AwardEntry>();

        public string? BestReviewer { get; set; }
    }
}
