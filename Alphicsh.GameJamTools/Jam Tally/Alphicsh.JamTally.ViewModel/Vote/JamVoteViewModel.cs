using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;
using Alphicsh.JamTally.ViewModel.Jam;
using Alphicsh.JamTally.ViewModel.Vote.Modals;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamTally.ViewModel.Vote
{
    public class JamVoteViewModel : WrapperViewModel<JamVote>
    {
        public static CollectionViewModelStub<JamVote, JamVoteViewModel> CollectionStub { get; }
            = new CollectionViewModelStub<JamVote, JamVoteViewModel>(jamVote => new JamVoteViewModel(jamVote));

        public JamVoteViewModel(JamVote model) : base(model)
        {
            VoterProperty = WrapperProperty
                .Create(this, nameof(Voter), vm => vm.Model.Voter, (vm, value) => vm.Model.Voter = value)
                .WithDependingProperty(nameof(DisplayVoter));
            AlignmentProperty = WrapperProperty
                .Create(this, nameof(Alignment), vm => vm.Model.Alignment, (vm, value) => vm.Model.Alignment = value);

            AuthoredEntries = CollectionViewModel.CreateMutable(model.Authored, JamVoteEntryViewModel.CollectionStub);
            RankingEntries = CollectionViewModel.CreateMutable(model.Ranking, JamVoteEntryViewModel.CollectionStub);
            UnjudgedEntries = CollectionViewModel.CreateMutable(model.Unjudged, JamVoteEntryViewModel.CollectionStub);
            UnrankedEntries = CollectionViewModel.CreateMutable(model.Missing, JamVoteEntryViewModel.CollectionStub);

            AwardSelections = JamTallyViewModel.Current.Jam!.Model.AwardCriteria
                .Select(criterion => new JamVoteAwardSelectionViewModel(Model, criterion))
                .ToList();

            DirectCountStringProperty = WrapperProperty.Create(
                this, nameof(DirectCountString),
                vm => vm.Model.HasDirectReviewsCount ? vm.Model.DirectReviewsCount!.Value.ToString() : string.Empty,
                (vm, value) => vm.Model.SetReviewsCountByString(value)
                );
            ReviewedEntries = CollectionViewModel.CreateMutable(model.ReviewedEntries, JamVoteEntryViewModel.CollectionStub);

            AutoFillAuthoredEntriesCommand = SimpleCommand.From(AutoFillAuthoredEntries);
            OpenEntriesEditorCommand = SimpleCommand.From(OpenEntriesEditor);

            AwardLines = Model.Awards.Select(award => $"{award.Criterion.Name}: {award.Entry.Line}").ToList();
            EntryLines = CalculateEntryLines();
            ReactionLines = Model.AggregateReactions.Select(reaction => $"+{reaction.Value} {reaction.Name}").ToList();
            ReactionScore = "Reaction score: " + Model.GetReactionScore();
        }

        // -----
        // Voter
        // -----

        public WrapperProperty<JamVoteViewModel, string> VoterProperty { get; }
        public string Voter { get => VoterProperty.Value; set => VoterProperty.Value = value; }
        public string DisplayVoter => !string.IsNullOrEmpty(Model.Voter) ? Model.Voter : "<unknown voter>";

        public WrapperProperty<JamVoteViewModel, JamAlignmentOption?> AlignmentProperty { get; }
        public JamAlignmentOption? Alignment { get => AlignmentProperty.Value; set => AlignmentProperty.Value = value; }
        public IReadOnlyCollection<JamAlignmentOptionViewModel> AvailableAlignments
            => JamTallyViewModel.Current.Jam!.AvailableAlignments;

        // -------
        // Entries
        // -------

        public CollectionViewModel<JamEntry, JamVoteEntryViewModel> AuthoredEntries { get; }
        public CollectionViewModel<JamEntry, JamVoteEntryViewModel> RankingEntries { get; }
        public CollectionViewModel<JamEntry, JamVoteEntryViewModel> UnjudgedEntries { get; }
        public CollectionViewModel<JamEntry, JamVoteEntryViewModel> UnrankedEntries { get; }

        // ------
        // Awards
        // ------

        public IReadOnlyCollection<JamVoteAwardSelectionViewModel> AwardSelections { get; }

        // -------
        // Reviews
        // -------

        public WrapperProperty<JamVoteViewModel, string> DirectCountStringProperty { get; }
        public string DirectCountString { get => DirectCountStringProperty.Value; set => DirectCountStringProperty.Value = value; }

        public CollectionViewModel<JamEntry, JamVoteEntryViewModel> ReviewedEntries { get; }

        // ----------
        // Management
        // ----------

        public ICommand AutoFillAuthoredEntriesCommand { get; }
        private void AutoFillAuthoredEntries()
            => JamTallyViewModel.Current.VoteManager.AutoFillVoteAuthoredEntries(this);

        public ICommand OpenEntriesEditorCommand { get; }
        private void OpenEntriesEditor()
            => VoteEntriesEditorViewModel.ShowModal(this);

        // ---------
        // Vote data
        // ---------

        public IReadOnlyCollection<string> AwardLines { get; set; }
        public IReadOnlyCollection<string> EntryLines { get; set; }
        public IReadOnlyCollection<string> ReactionLines { get; set; }
        public string ReactionScore { get; set; }

        private void ReloadVote()
        {
            AwardLines = Model.Awards.Select(award => $"{award.Criterion.Name}: {award.Entry.Line}").ToList();
            RaisePropertyChanged(nameof(AwardLines));

            EntryLines = CalculateEntryLines();
            RaisePropertyChanged(nameof(EntryLines));

            ReactionLines = Model.Reactions.Select(reaction => $"+{reaction.Value} {reaction.Name}").ToList();
            ReactionScore = "Reaction score: " + Model.GetReactionScore();
            RaisePropertyChanged(nameof(ReactionLines), nameof(ReactionScore));
        }

        private IReadOnlyCollection<string> CalculateEntryLines()
        {
            var lines = new List<string>();
            foreach (var entry in Model.Ranking)
            {
                lines.Add(entry.Line);
            }

            if (Model.Unjudged.Count > 0)
            {
                lines.Add("");
                lines.Add("Unjudged entries:");
                foreach (var entry in Model.Unjudged)
                {
                    lines.Add(entry.Line);
                }
            }

            if (Model.Missing.Count > 0)
            {
                lines.Add("");
                lines.Add("Missing entries:");
                foreach (var entry in Model.Missing)
                {
                    lines.Add(entry.Line);
                }
            }
            return lines;
        }
    }
}
