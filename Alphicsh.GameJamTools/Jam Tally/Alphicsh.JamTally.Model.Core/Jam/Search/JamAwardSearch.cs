using System;
using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Vote.Search
{
    internal class JamAwardSearch
    {
        private IReadOnlyDictionary<string, JamAwardCriterion> CriteriaByName { get; }
        private JamEntrySearch EntrySearch { get; }

        public JamAwardSearch(JamOverview jam, JamEntrySearch entrySearch)
        {
            CriteriaByName = MakeCriteriaByName(jam);
            EntrySearch = entrySearch;
        }

        // -----
        // Setup
        // -----

        private IReadOnlyDictionary<string, JamAwardCriterion> MakeCriteriaByName(JamOverview jam)
        {
            return jam.AwardCriteria.ToDictionary(criterion => criterion.Name, StringComparer.OrdinalIgnoreCase);
        }

        // ------
        // Search
        // ------

        public bool IsAwardWellFormed(string line)
        {
            return line.Contains(":");
        }

        public JamAwardCriterion? FindAwardCriterion(string line)
        {
            var separatorIdx = line.IndexOf(':');
            var criterionPart = line.Remove(separatorIdx).Trim();
            return CriteriaByName.TryGetValue(criterionPart, out var awardCriterion) ? awardCriterion : null;
        }

        public JamEntry? FindAwardEntry(string line)
        {
            var separatorIdx = line.IndexOf(':');
            var entryPart = line.Substring(separatorIdx + 1).Trim();
            return EntrySearch.FindEntry(entryPart, unprefixRanking: false);
        }
    }
}
