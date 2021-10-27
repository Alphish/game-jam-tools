using Alphicsh.JamPlayer.Model.Ranking;

using Alphicsh.JamTools.Common.Mvvm;

namespace Alphicsh.JamPlayer.ViewModel.Ranking
{
    public class RankingOverviewViewModel : BaseViewModel<RankingOverview>
    {
        public RankingOverviewViewModel(RankingOverview model)
            : base(model)
        {
            RankedEntries = new RankedEntriesListViewModel(model.RankedEntries, rankingOverview: this);
            UnrankedEntries = new UnrankedEntriesListViewModel(model.UnrankedEntries, rankingOverview: this);
        }

        public RankedEntriesListViewModel RankedEntries { get; }
        public UnrankedEntriesListViewModel UnrankedEntries { get; }
    }
}
