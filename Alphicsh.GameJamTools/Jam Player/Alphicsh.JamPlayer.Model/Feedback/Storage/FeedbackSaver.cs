using Alphicsh.JamPlayer.IO.Feedback;
using Alphicsh.JamPlayer.IO.Feedback.Storage;
using Alphicsh.JamTools.Common.IO.Storage.Saving;

namespace Alphicsh.JamPlayer.Model.Feedback.Storage
{
    internal class FeedbackSaver : BaseModelSaver<JamFeedback, FeedbackInfo>
    {
        private static FeedbackModelToInfoMapper Mapper { get; } = new FeedbackModelToInfoMapper();
        private static FeedbackInfoWriter Writer { get; } = new FeedbackInfoWriter();

        public FeedbackSaver() : base(Mapper, Writer)
        {
        }
    }
}
