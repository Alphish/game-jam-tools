using Alphicsh.JamTools.Common.IO.Saving;

namespace Alphicsh.JamPlayer.Model.Vote.Saving
{
    public class JamVoteSaveModel : SaveModel<JamVote, JamVoteSaveData>, ISaveModel<JamVote>
    {
        public JamVoteSaveModel() : base(
            new JamVoteSaveDataLoader(),
            new JamVoteSaveDataExtractor(),
            new JamVoteDataSaver()
            )
        {
        }
    }
}
