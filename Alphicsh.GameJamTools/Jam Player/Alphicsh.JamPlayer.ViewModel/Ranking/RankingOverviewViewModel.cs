using System.Linq;
using System.Windows.Input;

using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

using Alphicsh.JamPlayer.Model.Ranking;
using Alphicsh.JamPlayer.ViewModel.Ranking.Modals;

namespace Alphicsh.JamPlayer.ViewModel.Ranking
{
    public class RankingOverviewViewModel : WrapperViewModel<RankingOverview>
    {
        public RankingOverviewViewModel(RankingOverview model)
            : base(model)
        {
            RankedEntries = new RankedEntriesListViewModel(model.RankedEntries, rankingOverview: this);
            UnrankedEntries = new UnrankedEntriesListViewModel(model.UnrankedEntries, rankingOverview: this);

            HasPendingEntriesProperty = NotifiableProperty.Create(this, nameof(HasPendingEntriesProperty));
            PendingCountProperty = NotifiableProperty.Create(this, nameof(PendingCount));

            SelectedEntryProperty = MutableProperty.Create(this, nameof(SelectedEntry), initialValue: (RankingEntryViewModel?)null)
                .WithDependingProperty(RankedEntries, nameof(RankedEntries.SelectedEntry))
                .WithDependingProperty(UnrankedEntries, nameof(UnrankedEntries.SelectedEntry));

            GetNextEntryCommand = SimpleCommand.From(GetNextEntry);
            SearchEntryCommand = SimpleCommand.From(SearchEntry);
        }

        // ---------------
        // Pending entries
        // ---------------

        public NotifiableProperty PendingCountProperty { get; }
        public int PendingCount => Model.PendingEntries.Count;

        public NotifiableProperty HasPendingEntriesProperty { get; }
        public bool HasPendingEntries => Model.PendingEntries.Any();

        public ICommand GetNextEntryCommand { get; }
        private void GetNextEntry()
        {
            var modelEntry = Model.GetNextEntry();
            if (modelEntry == null)
                return;

            PendingCountProperty.RaisePropertyChanged();
            HasPendingEntriesProperty.RaisePropertyChanged();
            UnrankedEntries.SynchronizeWithModels();

            var viewModel = UnrankedEntries.First(vm => vm.Model == modelEntry);
            SelectedEntry = viewModel;
        }

        public ICommand SearchEntryCommand { get; }
        private void SearchEntry()
        {
            var searchViewModel = SearchEntryViewModel.ShowModal();
            var pickedEntry = searchViewModel.PickedEntry;
            if (pickedEntry == null)
                return;

            var rankingEntry = Model.PickEntry(pickedEntry.Entry.Model);

            PendingCountProperty.RaisePropertyChanged();
            HasPendingEntriesProperty.RaisePropertyChanged();
            UnrankedEntries.SynchronizeWithModels();

            var viewModel = UnrankedEntries.Concat(RankedEntries).First(vm => vm.Model == rankingEntry);
            
            SelectedEntry = viewModel;
        }

        public ICommand SaveRankingCommand => JamPlayerViewModel.Current.SaveRankingCommand;

        // ---------------
        // Ranking entries
        // ---------------

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
