using Alphicsh.JamPlayer.IO.Feedback;
using Alphicsh.JamTools.Common.IO.Saving;

namespace Alphicsh.JamPlayer.Model.Vote.Saving
{
    public class JamVoteSaveDataExtractor : ISaveDataExtractor<JamVote, JamVoteSaveData>
    {
        // TODO: add actual mapping
        public JamVoteSaveData ExtractData(JamVote model)
        {
            return new JamVoteSaveData()
            {
                DirectoryPath = AppModel.Current.PlayerDataManager.DirectoryPath,
                VoteInfo = new FeedbackInfo(),
            };
        }
    }
}
