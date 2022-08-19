using Alphicsh.JamPlayer.Model.Jam;

namespace Alphicsh.JamPlayer.Model.Awards
{
    public class AwardEntry
    {
        public JamAwardCriterion Criterion { get; init; } = default!;
        public JamEntry? JamEntry { get; set; }
    }
}
