using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamTally.Model.Jam
{
    public class JamOverview
    {
        // ------
        // Awards
        // ------

        private readonly IReadOnlyCollection<JamAwardCriterion> _awards = default!;
        private readonly IReadOnlyDictionary<string, JamAwardCriterion> _awardsByName = default!;

        public IReadOnlyCollection<JamAwardCriterion> AwardCriteria
        {
            get => _awards;
            init
            {
                _awards = value;
                _awardsByName = value.ToDictionary(award => award.Name, StringComparer.OrdinalIgnoreCase);
            }
        }

        public JamAwardCriterion? GetAwardByName(string? name)
        {
            if (name == null)
                return null;

            return _awardsByName.TryGetValue(name, out var award) ? award : null;
        }

        // -------
        // Entries
        // -------

        private readonly IReadOnlyCollection<JamEntry> _entries = default!;
        private readonly IReadOnlyDictionary<string, JamEntry> _entriesByTitles = default!;
        private readonly IReadOnlyDictionary<string, JamEntry> _entriesByLines = default!;

        public IReadOnlyCollection<JamEntry> Entries
        {
            get => _entries;
            init
            {
                _entries = value;
                _entriesByTitles = value.ToDictionary(entry => entry.Title, StringComparer.OrdinalIgnoreCase);
                _entriesByLines = value.ToDictionary(entry => entry.Line, StringComparer.OrdinalIgnoreCase);
            }
        }

        public JamEntry? GetEntryByTitle(string? title)
        {
            if (title == null)
                return null;

            return _entriesByTitles.TryGetValue(title, out var entry) ? entry : null;
        }

        public JamEntry? GetEntryByLine(string? line)
        {
            if (line == null)
                return null;

            return _entriesByLines.TryGetValue(line, out var entry) ? entry : null;
        }
    }
}
