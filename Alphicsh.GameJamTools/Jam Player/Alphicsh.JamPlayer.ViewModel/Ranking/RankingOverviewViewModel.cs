using Alphicsh.JamPlayer.Model.Ranking;

namespace Alphicsh.JamPlayer.ViewModel.Ranking
{
    public class RankingOverviewViewModel : BaseViewModel<RankingOverview>
    {
        public RankingOverviewViewModel(RankingOverview model)
            : base(model)
        {
            RankedEntries = CollectionViewModel.Create(model.RankedEntries, entry => new RankingEntryViewModel(entry));
            UnrankedEntries = CollectionViewModel.Create(model.UnrankedEntries, entry => new RankingEntryViewModel(entry));
        }

        public CollectionViewModel<RankingEntry, RankingEntryViewModel> RankedEntries { get; }
        public CollectionViewModel<RankingEntry, RankingEntryViewModel> UnrankedEntries { get; }
    }
}
