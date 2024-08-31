using Alphicsh.JamPlayer.IO.Feedback;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPlayer.Model.Vote.Saving
{
    public class JamVoteSaveData
    {
        public FilePath DirectoryPath { get; init; }
        public FeedbackInfo VoteInfo { get; init; } = default!;
    }
}
