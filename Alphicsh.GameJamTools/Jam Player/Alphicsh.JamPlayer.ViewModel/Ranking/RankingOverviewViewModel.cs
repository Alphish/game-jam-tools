using Alphicsh.JamPlayer.Model.Ranking;

using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPlayer.ViewModel.Ranking
{
    public class RankingOverviewViewModel : BaseViewModel<RankingOverview>
    {
        public RankingOverviewViewModel(RankingOverview model)
            : base(model)
        {
            RankedEntries = new RankedEntriesListViewModel(model.RankedEntries, rankingOverview: this);
            UnrankedEntries = new UnrankedEntriesListViewModel(model.UnrankedEntries, rankingOverview: this);

            SelectedEntryProperty = MutableProperty.Create(this, nameof(SelectedEntry), initialValue: (RankingEntryViewModel?)null)
                .WithDependingProperty(RankedEntries, nameof(RankedEntries.SelectedEntry))
                .WithDependingProperty(UnrankedEntries, nameof(UnrankedEntries.SelectedEntry));
        }

        // ------------------
        // Mutable properties
        // ------------------

        public RankedEntriesListViewModel RankedEntries { get; }
        public UnrankedEntriesListViewModel UnrankedEntries { get; }

        public MutableProperty<RankingEntryViewModel?> SelectedEntryProperty { get; }
        public RankingEntryViewModel? SelectedEntry { get => SelectedEntryProperty.Value; set => SelectedEntryProperty.Value = value; }
        public RankingEntryViewModel? GetRankedSelectedEntry()
        {
            return SelectedEntry != null && SelectedEntry.IsRanked ? SelectedEntry : null;
        }
        public RankingEntryViewModel? GetUnrankedSelectedEntry()
        {
            return SelectedEntry != null && !SelectedEntry.IsRanked ? SelectedEntry : null;
        }
    }
}
