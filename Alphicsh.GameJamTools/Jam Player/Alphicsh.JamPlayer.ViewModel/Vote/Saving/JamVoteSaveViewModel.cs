using Alphicsh.JamPlayer.Model.Vote;
using Alphicsh.JamPlayer.Model.Vote.Saving;
using Alphicsh.JamTools.Common.Mvvm.Saving;

namespace Alphicsh.JamPlayer.ViewModel.Vote.Saving
{
    public class JamVoteSaveViewModel : SaveViewModel<JamVote, JamVoteViewModel>
    {
        public JamVoteSaveViewModel() : base(
            new JamVoteSaveModel(),
            new JamVoteSaveDataObserver()
            )
        {
        }
    }
}
