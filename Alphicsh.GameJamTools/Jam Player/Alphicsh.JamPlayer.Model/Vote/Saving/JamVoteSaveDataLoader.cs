using Alphicsh.JamPlayer.IO.Feedback;
using Alphicsh.JamTools.Common.IO.Saving;

namespace Alphicsh.JamPlayer.Model.Vote.Saving
{
    public class JamVoteSaveDataLoader : ISaveDataLoader<JamVote, JamVoteSaveData>
    {
        public JamVoteSaveData? Load(JamVote model)
        {
            // TODO: add actual loading logic here
            var directoryPath = AppModel.Current.PlayerDataManager.DirectoryPath;
            return new JamVoteSaveData { DirectoryPath = directoryPath, VoteInfo = new FeedbackInfo() };
        }
    }
}
