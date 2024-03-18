using Alphicsh.JamTally.Model.Vote.Management;
using Alphicsh.JamTools.Common.Mvvm;

namespace Alphicsh.JamTally.ViewModel.Vote
{
    public class JamVoteManagerViewModel : WrapperViewModel<JamVoteManager>
    {
        public JamVoteManagerViewModel(JamVoteManager model) : base(model)
        {
        }

        public void AutoFillVoteAuthoredEntries(JamVoteViewModel vote)
        {
            var jam = JamTallyViewModel.Current.Jam!;
            Model.AutoFillVoteAuthoredEntries(vote.Model, jam.Model);

            vote.AuthoredEntries.SynchronizeWithModels();
            vote.RankingEntries.SynchronizeWithModels();
            vote.UnjudgedEntries.SynchronizeWithModels();
            vote.UnrankedEntries.SynchronizeWithModels();
        }
    }
}
