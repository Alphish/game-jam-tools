using System.Linq;
using System.Windows.Input;

using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

using Alphicsh.JamPlayer.Model.Ranking;

namespace Alphicsh.JamPlayer.ViewModel.Ranking
{
    public class RankingOverviewViewModel : WrapperViewModel<RankingOverview>
    {
        public RankingOverviewViewModel(RankingOverview model)
            : base(model)
        {
            RankedEntries = new RankedEntriesListViewModel(model.RankedEntries, rankingOverview: this);
            UnrankedEntries = new UnrankedEntriesListViewModel(model.UnrankedEntries, rankingOverview: this);

            SelectedEntryProperty = MutableProperty.Create(this, nameof(SelectedEntry), initialValue: (RankingEntryViewModel?)null)
                .WithDependingProperty(RankedEntries, nameof(RankedEntries.SelectedEntry))
                .WithDependingProperty(UnrankedEntries, nameof(UnrankedEntries.SelectedEntry));

            GetNextEntryCommand = new SimpleCommand(GetNextEntry);
        }

        // ---------------
        // Pending entries
        // ---------------

        public int PendingCount => Model.PendingEntries.Count;
        public bool HasPendingEntries => Model.PendingEntries.Any();
        public ICommand GetNextEntryCommand { get; }
        private void GetNextEntry()
        {
            var modelEntry = Model.GetNextEntry();
            if (modelEntry == null)
                return;

            RaisePropertyChanged(nameof(PendingCount));
            RaisePropertyChanged(nameof(HasPendingEntries));
            UnrankedEntries.SynchronizeWithModels();

            var viewModel = UnrankedEntries.First(vm => vm.Model == modelEntry);
            SelectedEntry = viewModel;
        }

        public ICommand SaveRankingCommand => AppViewModel.Current.SaveRankingCommand;

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
