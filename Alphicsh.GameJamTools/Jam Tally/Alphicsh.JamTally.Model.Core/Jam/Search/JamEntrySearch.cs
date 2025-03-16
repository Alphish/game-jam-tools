using System;
using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Vote.Search
{
    internal class JamEntrySearch
    {
        private IReadOnlyCollection<JamEntry> Entries { get; }
        private IReadOnlyDictionary<string, JamEntry?> EntriesByLine { get; }

        public JamEntrySearch(JamOverview jam)
        {
            Entries = jam.Entries;
            EntriesByLine = MakeEntriesByLine(jam);
        }

        // -----
        // Setup
        // -----

        private IReadOnlyDictionary<string, JamEntry?> MakeEntriesByLine(JamOverview jam)
        {
            var entries = jam.Entries;
            var result = new Dictionary<string, JamEntry?>(StringComparer.OrdinalIgnoreCase);

            PopulateDictionaryEntries(entries, result, entry => entry.FullLine);
            foreach (var kvp in result)
            {
                if (kvp.Value == null)
                    throw new InvalidOperationException($"Duplicate entry full line found: '{kvp.Key}'.");
            }

            PopulateDictionaryEntries(entries, result, entry => entry.Line);
            PopulateDictionaryEntries(entries, result, entry => entry.FullTitle);
            PopulateDictionaryEntries(entries, result, entry => entry.Title);

            return result;
        }

        private void PopulateDictionaryEntries(
            IEnumerable<JamEntry> entries,
            IDictionary<string, JamEntry?> dictionary,
            Func<JamEntry, string> keySelector
            )
        {
            var knownKeys = dictionary.Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var entry in entries)
            {
                var entryKey = keySelector(entry);
                if (knownKeys.Contains(entryKey))
                    continue;

                dictionary[entryKey] = !dictionary.ContainsKey(entryKey) ? entry : null;
            }
        }

        // ------
        // Search
        // ------

        public JamEntry? FindEntry(string line, bool unprefixRanking)
        {
            var byRawLine = EntriesByLine.TryGetValue(line, out var rawLineEntry) ? rawLineEntry : null;
            if (byRawLine != null)
                return byRawLine;

            if (!unprefixRanking)
                return null;

            var unprefixedLine = UnprefixRankingDigits(line);
            if (unprefixedLine == null)
                return null;

            var byUnprefixedLine = EntriesByLine.TryGetValue(unprefixedLine, out var unprefixedLineEntry) ? unprefixedLineEntry : null;
            return byUnprefixedLine;
        }

        private string? UnprefixRankingDigits(string line)
        {
            var idx = 0;
            var chars = line.ToCharArray();
            while (idx < chars.Length && char.IsDigit(chars[idx]))
                idx++;

            if (idx == 0)
                return null;

            if (chars[idx] == '.')
                idx++;

            return line.Substring(idx).Trim();
        }

        public IReadOnlyCollection<JamEntry> FindEntriesBy(string author)
        {
            return Entries
                .Where(entry => entry.Authors.Contains(author, StringComparer.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
