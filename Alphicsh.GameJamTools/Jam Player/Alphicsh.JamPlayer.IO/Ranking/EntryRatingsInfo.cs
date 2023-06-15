using System.Collections.Generic;

namespace Alphicsh.JamPlayer.IO.Ranking
{
    public class EntryRatingsInfo
    {
        public string EntryId { get; init; } = default!;
        public IReadOnlyCollection<RatingInfo> Ratings { get; init; } = default!;
        public string? Comment { get; init; }
        public bool? IsUnjudged { get; init; }
    }
}
