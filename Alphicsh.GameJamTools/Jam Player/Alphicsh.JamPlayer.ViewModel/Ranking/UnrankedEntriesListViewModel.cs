using System.Collections.Generic;

using Alphicsh.JamPlayer.Model.Ranking;

namespace Alphicsh.JamPlayer.ViewModel.Ranking
{
    public class UnrankedEntriesListViewModel : CollectionViewModel<RankingEntry, RankingEntryViewModel>
    {
        public RankingOverviewViewModel RankingOverview { get; }

        public UnrankedEntriesListViewModel(IList<RankingEntry> entries, RankingOverviewViewModel rankingOverview)
            : base(entries, RankingEntryViewModel.CollectionStub, isImmutable: false)
        {
            RankingOverview = rankingOverview;
        }

        protected override void ApplyChanges()
        {
            for (var i = 0; i < ViewModels.Count; i++)
            {
                var viewModel = ViewModels[i];
                viewModel.Rank = null;
            }
        }
    }
}
