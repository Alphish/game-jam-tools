using Alphicsh.JamTally.Model.Vote.Management;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.Modals;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;
using System.Windows.Input;

namespace Alphicsh.JamTally.ViewModel.Vote.Reactions
{
    public class VoteReactionsEditorViewModel : ModalViewModel
    {
        public JamVoteViewModel EditedVote { get; }
        public JamVoteReactionsEditor Model { get; }

        public VoteReactionsEditorViewModel(JamVoteViewModel editedVote)
            : base("Edit reactions for " + editedVote.DisplayVoter)
        {
            EditedVote = editedVote;
            Model = new JamVoteReactionsEditor(EditedVote.Model);

            ReactionsTextProperty = WrapperProperty.ForMember(this, vm => vm.Model.ReactionsText);
            AggregateReactionsTextProperty = WrapperProperty.ForReadonlyMember(this, vm => vm.Model.AggregateReactionsText);
            OutputProperty = WrapperProperty.ForReadonlyMember(this, vm => vm.Model.Output);

            ProcessCommand = SimpleCommand.From(ProcessInputs);
            ConfirmCommand = ConditionalCommand.From(Model.CanUpdateVote, Confirm)
                .ExecutionDependingOn(ReactionsTextProperty);
        }

        // -----
        // Modal
        // -----

        public static void ShowModal(JamVoteViewModel editedVote)
        {
            var viewModel = new VoteReactionsEditorViewModel(editedVote);
            viewModel.ShowOwnModal();
        }

        // ----
        // Text
        // ----

        public WrapperProperty<VoteReactionsEditorViewModel, string> ReactionsTextProperty { get; }
        public string ReactionsText { get => ReactionsTextProperty.Value; set => ReactionsTextProperty.Value = value; }

        public WrapperProperty<VoteReactionsEditorViewModel, string> AggregateReactionsTextProperty { get; }
        public string AggregateReactionsText => AggregateReactionsTextProperty.Value;

        public WrapperProperty<VoteReactionsEditorViewModel, string> OutputProperty { get; }
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
                nameof(ReactionsText), nameof(AggregateReactionsText),
                nameof(Output)
                );
        }

        public IConditionalCommand ConfirmCommand { get; }
        private void Confirm()
        {
            Model.UpdateVote();

            EditedVote.Reactions = EditedVote.CalculateReactions();
            EditedVote.RaisePropertyChanged(nameof(EditedVote.ReactionsHeader), nameof(EditedVote.Reactions));

            Window.Close();
        }
    }
}
