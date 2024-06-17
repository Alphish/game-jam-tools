using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;
using Alphicsh.JamTally.ViewModel.Jam;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamTally.ViewModel.Vote
{
    public class JamVoteAwardSelectionViewModel : BaseViewModel
    {
        public JamVoteAwardSelectionViewModel(JamVote vote, JamAwardCriterion criterion)
        {
            Vote = vote;
            Criterion = criterion;

            SelectedEntryProperty = WrapperProperty.Create(
                this, nameof(SelectedEntry),
                vm => vm.Vote.GetEntryForAward(Criterion),
                (vm, value) => vm.Vote.SetEntryForAward(Criterion, value)
                );
        }

        private JamVote Vote { get; }
        private JamAwardCriterion Criterion { get; }

        public string Name => Criterion.Name;

        public WrapperProperty<JamVoteAwardSelectionViewModel, JamEntry?> SelectedEntryProperty { get; }
        public JamEntry? SelectedEntry { get => SelectedEntryProperty.Value; set => SelectedEntryProperty.Value = value; }
        public IReadOnlyCollection<JamEntryOptionViewModel> AvailableEntries
            => JamTallyViewModel.Current.Jam!.AvailableEntries;
    }
}
