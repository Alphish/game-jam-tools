using Alphicsh.JamPlayer.Model.Jam;
using Alphicsh.JamPlayer.Model.Ratings;
using Alphicsh.JamTools.Common.IO.Storage.Loading;

namespace Alphicsh.JamPlayer.Model.Feedback.Loading
{
    public class FeedbackLoadModel : LoadModel<JamFeedback>
    {
        private static JamFeedback BlankModel { get; } = new JamFeedback
        {
            Ranking = new Ranking.RankingOverview(),
            Awards = new Awards.AwardsOverview(),
        };

        public FeedbackLoadModel(JamOverview jam, RatingCriteriaOverview ratingCriteria)
            : base(new FeedbackLoader(jam, ratingCriteria), BlankModel)
        {
        }

        public void UpdateContext(JamOverview jam, RatingCriteriaOverview ratingCriteria)
        {
            var loader = new FeedbackLoader(jam, ratingCriteria);
            ReplaceLoader(loader);
        }
    }
}
