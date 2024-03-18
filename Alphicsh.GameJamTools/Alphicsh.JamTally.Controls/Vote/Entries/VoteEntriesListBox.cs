using System.Windows;
using Alphicsh.JamTally.ViewModel.Vote;
using Alphicsh.JamTools.Common.Controls;

namespace Alphicsh.JamTally.Controls.Vote.Entries
{
    internal class VoteEntriesListBox : ReorderableListBox<JamVoteEntryViewModel>
    {
        public static readonly DependencyProperty VoteProperty
            = DependencyProperty.Register(nameof(Vote), typeof(JamVoteViewModel), typeof(VoteEntriesListBox));

        public JamVoteViewModel Vote
        {
            get => (JamVoteViewModel)GetValue(VoteProperty);
            set => SetValue(VoteProperty, value);
        }

        public override void AfterDrop(JamVoteEntryViewModel item)
        {
            Vote.AuthoredEntries.CompleteChanges();
            Vote.RankingEntries.CompleteChanges();
            Vote.UnjudgedEntries.CompleteChanges();
            Vote.UnrankedEntries.CompleteChanges();
        }
    }
}
