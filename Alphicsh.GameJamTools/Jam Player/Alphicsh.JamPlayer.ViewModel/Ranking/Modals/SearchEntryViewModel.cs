using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTools.Common.Mvvm.Modals;
using Alphicsh.JamPlayer.ViewModel.Jam;
using System.Text.RegularExpressions;

namespace Alphicsh.JamPlayer.ViewModel.Ranking.Modals
{
    public class SearchEntryViewModel : ModalViewModel
    {
        public SearchEntryViewModel()
            : base("Search entry")
        {
            SearchableEntries = GetAllSearchableEntries();
        }

        public static SearchEntryViewModel ShowModal()
        {
            var viewModel = new SearchEntryViewModel();
            viewModel.ShowOwnModal();
            return viewModel;
        }

        // ---------------
        // Listing entries
        // ---------------

        public SearchableEntryViewModel? PickedEntry { get; private set; }
        public IReadOnlyCollection<SearchableEntryViewModel> SearchableEntries { get; }

        private IReadOnlyCollection<SearchableEntryViewModel> GetAllSearchableEntries()
        {
            var rankedEntries = JamPlayerViewModel.Current.Ranking.RankedEntries.Select(rankingEntry => rankingEntry.JamEntry);
            var unrankedEntries = JamPlayerViewModel.Current.Ranking.UnrankedEntries.Select(rankingEntry => rankingEntry.JamEntry);
            var listedEntries = rankedEntries.Concat(unrankedEntries)
                .Select(entry => new SearchableEntryViewModel(entry, isListed: true, this)).ToList();

            var pendingEntries = JamPlayerViewModel.Current.Ranking.Model.PendingEntries
                .Select(pendingEntry => new JamEntryViewModel(pendingEntry.JamEntry))
                .Select(entry => new SearchableEntryViewModel(entry, isListed: false, this)).ToList();

            return listedEntries.Concat(pendingEntries)
                .OrderBy(entry => entry.Entry.Title)
                .ThenBy(entry => entry.Entry.Team.ShortDescription)
                .ToList();
        }

        // ----------------
        // Querying entries
        // ----------------

        private string QueryStringUnderlying { get; set; } = "";
        public string QueryString
        {
            get => QueryStringUnderlying;
            set
            {
                if (value == QueryStringUnderlying)
                    return;

                QueryStringUnderlying = value;
                FilteredEntries = FilterEntries();
                RaisePropertyChanged(nameof(QueryString));
                RaisePropertyChanged(nameof(FilteredEntries));
            }
        }

        public IReadOnlyCollection<SearchableEntryViewModel> FilteredEntries { get; private set; } = new List<SearchableEntryViewModel>();
        private IReadOnlyCollection<SearchableEntryViewModel> FilterEntries()
        {
            var queryWords = Regex.Matches(QueryStringUnderlying, "\\w+").OfType<Match>()
                .Select(match => match.Value.ToLowerInvariant())
                .Where(keyword => keyword != "by")
                .ToList();

            if (queryWords.Count == 0 || (queryWords.Count == 1 && queryWords.First().Length < 3))
                return new List<SearchableEntryViewModel>();

            return SearchableEntries.Where(entry => entry.MatchQuery(queryWords)).ToList();
        }

        // -----------------
        // Picking the entry
        // -----------------

        internal void PickEntry(SearchableEntryViewModel entry)
        {
            PickedEntry = entry;
            Window.Close();
        }
    }
}
