using Alphicsh.JamTools.Common.Controls;

using Alphicsh.JamPlayer.ViewModel.Ranking;
using System.Windows;

namespace Alphicsh.JamPlayer.Controls.Ranking
{
    public class RankingListBox : ReorderableListBox<RankingEntryViewModel>
    {
        public static readonly DependencyProperty RankingOverviewProperty
            = DependencyProperty.Register(nameof(RankingOverview), typeof(RankingOverviewViewModel), typeof(RankingListBox));

        public RankingOverviewViewModel RankingOverview
        {
            get => (RankingOverviewViewModel)GetValue(RankingOverviewProperty);
            set => SetValue(RankingOverviewProperty, value);
        }

        public override void AfterDrop(RankingEntryViewModel item)
        {
            RankingOverview.RankedEntries.CompleteChanges();
            RankingOverview.UnrankedEntries.CompleteChanges();

            this.SelectedItem = item;
            RankingOverview.SelectedEntry = item;
        }
    }
}
