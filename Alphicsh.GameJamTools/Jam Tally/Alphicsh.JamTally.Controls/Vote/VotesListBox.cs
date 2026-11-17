using System.Windows;
using Alphicsh.JamTally.ViewModel.Vote;
using Alphicsh.JamTools.Common.Controls;

namespace Alphicsh.JamTally.Controls.Vote
{
    public class VotesListBox : ReorderableListBox<JamVoteViewModel>
    {
        public static readonly DependencyProperty VoteCollectionProperty
            = DependencyProperty.Register(nameof(VoteCollection), typeof(JamVoteCollectionViewModel), typeof(VotesListBox));

        public JamVoteCollectionViewModel VoteCollection
        {
            get => (JamVoteCollectionViewModel)GetValue(VoteCollectionProperty);
            set => SetValue(VoteCollectionProperty, value);
        }

        public override void AfterDrop(JamVoteViewModel item)
        {
            VoteCollection.Votes.CompleteChanges();

            this.SelectedItem = item;
            VoteCollection.SelectedVote = item;
        }
    }
}
