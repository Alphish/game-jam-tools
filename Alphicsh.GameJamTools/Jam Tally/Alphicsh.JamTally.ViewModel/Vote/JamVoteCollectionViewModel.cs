using System.Windows.Input;
using Alphicsh.JamTally.Model.Vote;
using Alphicsh.JamTally.ViewModel.Result;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamTally.ViewModel.Vote
{
    public class JamVoteCollectionViewModel : WrapperViewModel<JamVoteCollection>
    {
        public JamVoteCollectionViewModel(JamVoteCollection model) : base(model)
        {
            Votes = CollectionViewModel.CreateMutable(model.Votes, JamVoteViewModel.CollectionStub);
            SelectedVoteProperty = MutableProperty.Create(this, nameof(SelectedVote), initialValue: (JamVoteViewModel?)null);

            AddVoteCommand = SimpleCommand.From(AddVote);
            RemoveVoteCommand = SimpleCommand.WithParameter<JamVoteViewModel>(RemoveVote);
            SaveVotesCommand = SimpleCommand.From(SaveVotes);
            TallyVotesCommand = SimpleCommand.From(TallyVotes);
        }

        public CollectionViewModel<JamVote, JamVoteViewModel> Votes { get; }
        public MutableProperty<JamVoteViewModel?> SelectedVoteProperty { get; }
        public JamVoteViewModel? SelectedVote
        {
            get => SelectedVoteProperty.Value;
            set => SelectedVoteProperty.Value = value;
        }

        public ICommand AddVoteCommand { get; }
        private void AddVote()
        {
            Model.AddVote();
            Votes.SynchronizeWithModels();
            Votes.CompleteChanges();
        }

        public ICommand RemoveVoteCommand { get; }
        private void RemoveVote(JamVoteViewModel vote)
        {
            Model.RemoveVote(vote.Model);
            Votes.SynchronizeWithModels();
            Votes.CompleteChanges();
        }

        public ICommand SaveVotesCommand { get; }
        private void SaveVotes()
        {
            Model.SaveVotes();
        }

        // -----
        // Tally
        // -----

        public bool HasTallyResults => Model.HasTallyResult;
        public JamTallyResultViewModel? TallyResult { get; private set; }

        public ICommand TallyVotesCommand { get; }
        private void TallyVotes()
        {
            Model.TallyVotes();
            TallyResult = new JamTallyResultViewModel(Model.TallyResult!);
            RaisePropertyChanged(nameof(TallyResult), nameof(HasTallyResults));
        }
    }
}
