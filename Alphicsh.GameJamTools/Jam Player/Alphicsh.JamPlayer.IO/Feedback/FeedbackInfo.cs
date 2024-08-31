using System.Collections.Generic;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPlayer.IO.Feedback
{
    public class FeedbackInfo
    {
        public static readonly string Filename = "feedback.jamfeedback";

        public FilePath Location { get; set; } = default!;

        public IReadOnlyCollection<FeedbackEntryInfo> Entries { get; init; } = new List<FeedbackEntryInfo>();
        public FeedbackRankingInfo Ranking { get; init; } = default!;
        public FeedbackAwardsInfo Awards { get; init; } = default!;
    }
}
