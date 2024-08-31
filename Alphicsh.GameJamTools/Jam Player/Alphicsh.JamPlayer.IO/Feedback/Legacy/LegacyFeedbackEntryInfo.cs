using System.Collections.Generic;

namespace Alphicsh.JamPlayer.IO.Feedback.Legacy
{
    internal class LegacyFeedbackEntryInfo
    {
        public string EntryId { get; init; } = default!;
        public IReadOnlyCollection<LegacyFeedbackRatingInfo> Ratings { get; init; } = default!;
        public string? Comment { get; init; }
        public bool? IsUnjudged { get; init; }
    }
}
