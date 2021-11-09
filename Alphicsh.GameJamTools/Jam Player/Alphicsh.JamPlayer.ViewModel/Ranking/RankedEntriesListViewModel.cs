﻿using System.Collections.Generic;

using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

using Alphicsh.JamPlayer.Model.Ranking;

namespace Alphicsh.JamPlayer.ViewModel.Ranking
{
    public class RankedEntriesListViewModel : CollectionViewModel<RankingEntry, RankingEntryViewModel>
    {
        public RankingOverviewViewModel RankingOverview { get; }

        public RankedEntriesListViewModel(IList<RankingEntry> entries, RankingOverviewViewModel rankingOverview)
            : base(entries, RankingEntryViewModel.CollectionStub, isImmutable: false)
        {
            RankingOverview = rankingOverview;

            SelectedEntryProperty = WrapperProperty.Create(
                this, nameof(SelectedEntry),
                valueGetter: vm => vm.RankingOverview.GetRankedSelectedEntry(),
                valueSetter: (vm, value) => vm.RankingOverview.SelectedEntry = value
                );
        }

        protected override void ApplyChanges()
        {
            for (var i = 0; i < ViewModels.Count; i++)
            {
                var viewModel = ViewModels[i];
                viewModel.Rank = i + 1;
            }
        }

        public WrapperProperty<RankedEntriesListViewModel, RankingEntryViewModel?> SelectedEntryProperty { get; }
        public RankingEntryViewModel? SelectedEntry { get => SelectedEntryProperty.Value; set => SelectedEntryProperty.Value = value; }
    }
}
