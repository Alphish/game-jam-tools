using System;
using System.Collections.Generic;
using System.Linq;

using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPlayer.Model.Jam
{
    public class JamOverview
    {
        public FilePath DirectoryPath { get; init; } = default!;

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

        public IReadOnlyCollection<JamAwardCriterion> AwardCriteria { get; init; } = default!;
    }
}
