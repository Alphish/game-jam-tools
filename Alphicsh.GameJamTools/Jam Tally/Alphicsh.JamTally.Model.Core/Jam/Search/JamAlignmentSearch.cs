using System;
using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Vote.Search
{
    internal class JamAlignmentSearch
    {
        private bool HasAlignments { get; }
        private IReadOnlyDictionary<string, JamAlignmentOption?> AlignmentsByName { get; }

        public JamAlignmentSearch(JamOverview jam)
        {
            HasAlignments = jam.Alignments != null;
            AlignmentsByName = MakeAlignmentsByName(jam);
        }

        // -----
        // Setup
        // -----

        private IReadOnlyDictionary<string, JamAlignmentOption?> MakeAlignmentsByName(JamOverview jam)
        {
            var result = new Dictionary<string, JamAlignmentOption?>(StringComparer.OrdinalIgnoreCase);
            var alignments = jam.Alignments;
            if (alignments == null)
                return result;

            result.Add(alignments.NeitherTitle, null);
            foreach (var option in alignments.GetAvailableOptions())
            {
                result.Add(option.Title, option);
                result.Add(option.ShortTitle, option);
            }

            return result;
        }

        // ------
        // Search
        // ------

        public bool AlignmentsEnabled => HasAlignments;

        public bool IsAlignmentValid(string line)
            => AlignmentsByName.ContainsKey(line.Trim());

        public JamAlignmentOption? FindAlignment(string line)
            => AlignmentsByName.TryGetValue(line.Trim(), out var alignment) ? alignment : null;
    }
}
