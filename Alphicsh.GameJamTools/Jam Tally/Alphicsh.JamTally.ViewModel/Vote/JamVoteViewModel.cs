﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;
using Alphicsh.JamTally.ViewModel.Jam;
using Alphicsh.JamTally.ViewModel.Vote.Modals;
using Alphicsh.JamTally.ViewModel.Vote.Reactions;
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

            Reactions = CalculateReactions();

            AutoFillAuthoredEntriesCommand = SimpleCommand.From(AutoFillAuthoredEntries);
            OpenEntriesEditorCommand = SimpleCommand.From(OpenEntriesEditor);
            OpenReactionsEditorCommand = SimpleCommand.From(OpenReactionsEditor);
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

        // ---------
        // Reactions
        // ---------

        public string ReactionsHeader => $"Reaction score: {Model.GetReactionScore()}";
        public IReadOnlyCollection<JamVoteReactionViewModel> Reactions { get; set; }

        public IReadOnlyCollection<JamVoteReactionViewModel> CalculateReactions()
        {
            return Model.Reactions
                .Select(reaction => new JamVoteReactionViewModel(reaction, Model.AggregateReactions.Contains(reaction)))
                .OrderBy(reaction => reaction.User)
                .ThenByDescending(reaction => reaction.IsCounted ? 1 : 0)
                .ThenByDescending(reaction => reaction.Value)
                .ToList();
        }

        // ----------
        // Management
        // ----------

        public ICommand AutoFillAuthoredEntriesCommand { get; }
        private void AutoFillAuthoredEntries()
            => JamTallyViewModel.Current.VoteManager.AutoFillVoteAuthoredEntries(this);

        public ICommand OpenEntriesEditorCommand { get; }
        private void OpenEntriesEditor()
            => VoteEntriesEditorViewModel.ShowModal(this);

        public ICommand OpenReactionsEditorCommand { get; }
        private void OpenReactionsEditor()
            => VoteReactionsEditorViewModel.ShowModal(this);
    }
}
