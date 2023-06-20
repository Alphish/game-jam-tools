using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamTally.Model.Jam
{
    public class JamOverview
    {
        public IReadOnlyCollection<JamAwardCriterion> AwardCriteria { get; init; } = default!;

        // -------
        // Entries
        // -------

        private readonly IReadOnlyCollection<JamEntry> _entries = default!;
        private readonly IReadOnlyDictionary<string, JamEntry> _entriesById = default!;

        public IReadOnlyCollection<JamEntry> Entries
        {
            get => _entries;
            init
            {
                _entries = value;
                _entriesById = value.ToDictionary(entry => entry.Id, StringComparer.OrdinalIgnoreCase);
            }
        }

        public JamEntry? GetEntryById(string? entryId)
        {
            if (entryId == null)
                return null;

            return _entriesById.TryGetValue(entryId, out var entry) ? entry : null;
        }
    }
}
