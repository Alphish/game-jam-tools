using System.Collections.Generic;

using Alphicsh.JamPlayer.Model.Jam;
using Alphicsh.JamPlayer.Model.Ratings;

namespace Alphicsh.JamPlayer.Model.Ranking
{
    public sealed class RankingEntry
    {
        public JamEntry JamEntry { get; init; } = default!;
        public IReadOnlyCollection<IRating> Ratings { get; init; } = default!;
        public string Comment { get; set; } = string.Empty;
        public int? Rank { get; set; } = default!;
    }
}
