using Alphicsh.JamPlayer.Model.Awards;
using Alphicsh.JamPlayer.Model.Ranking;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPlayer.Model.Feedback
{
    public class JamFeedback
    {
        public FilePath Location { get; internal set; } = default!;

        public RankingOverview Ranking { get; internal set; } = new RankingOverview();
        public AwardsOverview Awards { get; internal set; } = new AwardsOverview();
    }
}
