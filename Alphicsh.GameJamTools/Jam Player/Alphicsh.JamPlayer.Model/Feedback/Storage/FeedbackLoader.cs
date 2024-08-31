using Alphicsh.JamPlayer.IO.Feedback;
using Alphicsh.JamPlayer.IO.Feedback.Storage;
using Alphicsh.JamPlayer.Model.Jam;
using Alphicsh.JamPlayer.Model.Ratings;
using Alphicsh.JamTools.Common.IO.Storage;
using Alphicsh.JamTools.Common.IO.Storage.Loading;

namespace Alphicsh.JamPlayer.Model.Feedback.Loading
{
    internal class FeedbackLoader : BaseModelLoader<FeedbackInfo, FeedbackInfo, JamFeedback>
    {
        private static FeedbackInfoReader InfoReader { get; } = new FeedbackInfoReader();

        public FeedbackLoader(JamOverview jam, RatingCriteriaOverview ratingCriteria)
            : base(InfoReader, CreateMapper(jam, ratingCriteria), fixBeforeLoading: true)
        {
        }

        private static IMapper<FeedbackInfo, JamFeedback> CreateMapper(JamOverview jam, RatingCriteriaOverview ratingCriteria)
            => new FeedbackInfoToModelMapper(jam, ratingCriteria);
    }
}
