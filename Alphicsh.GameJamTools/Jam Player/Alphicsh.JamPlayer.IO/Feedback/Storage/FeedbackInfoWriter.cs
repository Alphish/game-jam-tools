using Alphicsh.JamTools.Common.IO.Storage;
using Alphicsh.JamTools.Common.IO.Storage.Formats;
using Alphicsh.JamTools.Common.IO.Storage.Saving;

namespace Alphicsh.JamPlayer.IO.Feedback.Storage
{
    public class FeedbackInfoWriter : BaseFileModelWriter<FeedbackInfo>
    {
        private static JsonFormatter<FeedbackInfo> FeedbackFormatter { get; } = new JsonFormatter<FeedbackInfo>();

        protected override FileData SerializeCore(FeedbackInfo info)
        {
            var content = FeedbackFormatter.Format(info);
            return new FileData(info.Location, content);
        }
    }
}
