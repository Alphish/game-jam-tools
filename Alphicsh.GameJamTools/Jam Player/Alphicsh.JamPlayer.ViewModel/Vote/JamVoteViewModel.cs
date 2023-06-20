using Alphicsh.JamPlayer.Model.Vote;
using Alphicsh.JamPlayer.ViewModel.Awards;
using Alphicsh.JamPlayer.ViewModel.Ranking;
using Alphicsh.JamTools.Common.Mvvm;

namespace Alphicsh.JamPlayer.ViewModel.Vote
{
    public class JamVoteViewModel : WrapperViewModel<JamVote>
    {
        public JamVoteViewModel(JamPlayerViewModel playerViewModel, JamVote model) : base(model)
        {
            PlayerViewModel = playerViewModel;
        }

        public JamPlayerViewModel PlayerViewModel { get; }
        public RankingOverviewViewModel Ranking => PlayerViewModel.Ranking;
        public AwardsOverviewViewModel Awards => PlayerViewModel.Awards;
    }
}
