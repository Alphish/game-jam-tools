using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTools.Common.Mvvm;

namespace Alphicsh.JamTally.ViewModel.Vote
{
    public class JamVoteEntryViewModel : WrapperViewModel<JamEntry>
    {
        public static CollectionViewModelStub<JamEntry, JamVoteEntryViewModel> CollectionStub { get; }
            = CollectionViewModelStub.Create((JamEntry model) => new JamVoteEntryViewModel(model));

        public JamVoteEntryViewModel(JamEntry model) : base(model)
        {
        }

        public string Title => Model.Title;
        public string Team => Model.Team;
    }
}
