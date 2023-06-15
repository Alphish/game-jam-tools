using Alphicsh.JamTools.Common.IO.Saving;

namespace Alphicsh.JamPlayer.Model.Vote.Saving
{
    public class JamVoteDataSaver : IDataSaver<JamVoteSaveData>
    {
        public void Save(JamVoteSaveData saveData)
        {
            // TODO: move ranking saving logic to here
            AppModel.Current.PlayerDataManager.SaveRanking();
        }
    }
}
