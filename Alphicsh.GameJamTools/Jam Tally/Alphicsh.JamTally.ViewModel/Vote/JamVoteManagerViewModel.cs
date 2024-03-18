using Alphicsh.JamTally.Model.Vote.Management;
using Alphicsh.JamTally.ViewModel.Jam;
using Alphicsh.JamTools.Common.Mvvm;

namespace Alphicsh.JamTally.ViewModel.Vote
{
    public class JamVoteManagerViewModel : WrapperViewModel<JamVoteManager>
    {
        private JamOverviewViewModel Jam => JamTallyViewModel.Current.Jam!;

        public JamVoteManagerViewModel(JamVoteManager model) : base(model)
        {
        }

        public void AutoFillVoteAuthoredEntries(JamVoteViewModel vote)
        {
            Model.AutoFillVoteAuthoredEntries(vote.Model, Jam.Model);

            vote.AuthoredEntries.SynchronizeWithModels();
            vote.RankingEntries.SynchronizeWithModels();
            vote.UnjudgedEntries.SynchronizeWithModels();
            vote.UnrankedEntries.SynchronizeWithModels();
        }
    }
}
