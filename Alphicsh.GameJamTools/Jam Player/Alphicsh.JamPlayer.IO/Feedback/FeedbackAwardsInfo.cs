using System.Collections.Generic;

namespace Alphicsh.JamPlayer.IO.Feedback
{
    public class FeedbackAwardsInfo
    {
        public IReadOnlyCollection<FeedbackAwardNominationInfo> Nominations { get; init; } = new List<FeedbackAwardNominationInfo>();
    }
}
