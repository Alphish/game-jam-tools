using System.Collections.Generic;

namespace Alphicsh.JamPlayer.IO.Feedback
{
    public class FeedbackRankingInfo
    {
        public IReadOnlyCollection<string> RankedEntries { get; init; } = default!;
        public IReadOnlyCollection<string> UnrankedEntries { get; init; } = default!;
    }
}
