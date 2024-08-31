using System.Collections.Generic;

namespace Alphicsh.JamPlayer.IO.Feedback
{
    public class FeedbackEntryInfo
    {
        public string EntryId { get; init; } = default!;
        public IReadOnlyCollection<FeedbackRatingInfo> Ratings { get; init; } = new List<FeedbackRatingInfo>();
        public string? Comment { get; init; }
        public bool? IsUnjudged { get; init; }
    }
}
