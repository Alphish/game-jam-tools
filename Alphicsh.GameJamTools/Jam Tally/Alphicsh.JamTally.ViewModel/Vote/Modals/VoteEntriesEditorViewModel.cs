using System.Windows.Input;
using Alphicsh.JamTally.Model.Vote.Management;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.Modals;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamTally.ViewModel.Vote.Modals
{
    public class VoteEntriesEditorViewModel : ModalViewModel
    {
        public JamVoteViewModel EditedVote { get; }
        public JamVoteEntriesEditor Model { get; }

        public VoteEntriesEditorViewModel(JamVoteViewModel editedVote)
            : base("Edit vote by " + editedVote.DisplayVoter)
        {
            EditedVote = editedVote;
            Model = new JamVoteEntriesEditor(EditedVote.Model);

            AuthoredTextProperty = WrapperProperty.ForMember(this, vm => vm.Model.AuthoredText);
            RankingTextProperty = WrapperProperty.ForMember(this, vm => vm.Model.RankingText);
            UnjudgedTextProperty = WrapperProperty.ForMember(this, vm => vm.Model.UnjudgedText);
            AwardsTextProperty = WrapperProperty.ForMember(this, vm => vm.Model.AwardsText);
            ReviewedTextProperty = WrapperProperty.ForMember(this, vm => vm.Model.ReviewedText);
            OutputProperty = WrapperProperty.ForReadonlyMember(this, vm => vm.Model.Output);

            ProcessCommand = SimpleCommand.From(ProcessInputs);
            ConfirmCommand = ConditionalCommand.From(Model.CanUpdateVote, Confirm)
                .ExecutionDependingOn(AuthoredTextProperty, RankingTextProperty, UnjudgedTextProperty)
                .ExecutionDependingOn(AwardsTextProperty, ReviewedTextProperty);
        }

        // -----
        // Modal
        // -----

        public static void ShowModal(JamVoteViewModel editedVote)
        {
            var viewModel = new VoteEntriesEditorViewModel(editedVote);
            viewModel.ShowOwnModal();
        }

        // ----
        // Text
        // ----

        public WrapperProperty<VoteEntriesEditorViewModel, string> AuthoredTextProperty { get; }
        public string AuthoredText { get => AuthoredTextProperty.Value; set => AuthoredTextProperty.Value = value; }

        public WrapperProperty<VoteEntriesEditorViewModel, string> RankingTextProperty { get; }
        public string RankingText { get => RankingTextProperty.Value; set => RankingTextProperty.Value = value; }

        public WrapperProperty<VoteEntriesEditorViewModel, string> UnjudgedTextProperty { get; }
        public string UnjudgedText { get => UnjudgedTextProperty.Value; set => UnjudgedTextProperty.Value = value; }

        public WrapperProperty<VoteEntriesEditorViewModel, string> AwardsTextProperty { get; }
        public string AwardsText { get => AwardsTextProperty.Value; set => AwardsTextProperty.Value = value; }

        public WrapperProperty<VoteEntriesEditorViewModel, string> ReviewedTextProperty { get; }
        public string ReviewedText { get => ReviewedTextProperty.Value; set => ReviewedTextProperty.Value = value; }

        public WrapperProperty<VoteEntriesEditorViewModel, string> OutputProperty { get; }
        public string Output => OutputProperty.Value;

        // --------
        // Commands
        // --------

        public ICommand ProcessCommand { get; }
        private void ProcessInputs()
        {
            Model.ProcessInputs();
            ConfirmCommand.RaiseCanExecuteChanged();

            RaisePropertyChanged(
                nameof(AuthoredText), nameof(RankingText), nameof(UnjudgedText),
                nameof(AwardsText), nameof(ReviewedText),
                nameof(Output)
                );
        }

        public IConditionalCommand ConfirmCommand { get; }
        private void Confirm()
        {
            Model.UpdateVote();

            EditedVote.AuthoredEntries.SynchronizeWithModels();
            EditedVote.RankingEntries.SynchronizeWithModels();
            EditedVote.UnjudgedEntries.SynchronizeWithModels();
            EditedVote.UnrankedEntries.SynchronizeWithModels();

            foreach (var awardSelection in EditedVote.AwardSelections)
                awardSelection.RaisePropertyChanged(nameof(awardSelection.SelectedEntry));

            Window.Close();
        }


    }
}
