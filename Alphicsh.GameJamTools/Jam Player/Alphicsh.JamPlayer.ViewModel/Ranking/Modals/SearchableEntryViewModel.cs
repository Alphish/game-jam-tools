using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Alphicsh.JamPlayer.ViewModel.Jam;
using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.JamPlayer.ViewModel.Ranking.Modals
{
    public class SearchableEntryViewModel
    {
        public JamEntryViewModel Entry { get; }
        public bool IsListed { get; }
        public SearchEntryViewModel Modal { get; }

        public SearchableEntryViewModel(JamEntryViewModel entry, bool isListed, SearchEntryViewModel modal)
        {
            Entry = entry;
            IsListed = isListed;
            Modal = modal;

            Keywords = ExtractKeywords();
            PickEntryCommand = SimpleCommand.From(PickEntry);
        }

        // --------
        // Keywords
        // --------

        public HashSet<string> Keywords { get; }
        
        private HashSet<string> ExtractKeywords()
        {
            var result = new HashSet<string>();
            result.Add("by"); // matching queries like "Best Game EVA by dadio"

            foreach (var keyword in GetKeywordsStream(Entry.Title))
                result.Add(keyword);
            
            foreach (var keyword in GetKeywordsStream(Entry.Team.Name))
                result.Add(keyword);

            foreach (var author in Entry.Team.Authors)
            {
                foreach (var keyword in GetKeywordsStream(author.Name))
                    result.Add(keyword);
            }

            return result;
        }

        private IEnumerable<string> GetKeywordsStream(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                yield break;

            foreach (var match in Regex.Matches(input, "\\w+").OfType<Match>())
                yield return match.Value.ToLowerInvariant();
        }

        // --------
        // Querying
        // --------

        public bool MatchQuery(IList<string> query)
        {
            for (var i = 0; i < query.Count; i++)
            {
                var word = query[i];
                if (i < query.Count - 1 && !Keywords.Contains(word))
                    return false;
                else if (!Keywords.Any(keyword => keyword.StartsWith(word)))
                    return false;
            }
            return true;
        }

        // -------
        // Picking
        // -------

        public ICommand PickEntryCommand { get; }
        public void PickEntry()
        {
            Modal.PickEntry(this);
        }

        public string PickEntryDescription => IsListed ? "Select" : "Add to list";
    }
}
