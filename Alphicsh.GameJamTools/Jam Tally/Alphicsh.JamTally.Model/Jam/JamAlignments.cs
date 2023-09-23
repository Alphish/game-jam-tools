using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamTally.Model.Jam
{
    public class JamAlignments
    {
        private IReadOnlyCollection<JamAlignmentOption> AvailableOptions { get; } = default!;
        private IReadOnlyDictionary<string, JamAlignmentOption?> Lookup { get; } = default!;

        public JamAlignments(string neitherTitle, IEnumerable<JamAlignmentOption> options)
        {
            AvailableOptions = options.ToList();
            Lookup = CreateLookup(neitherTitle, options);
        }

        private IReadOnlyDictionary<string, JamAlignmentOption?> CreateLookup(string neitherTitle, IEnumerable<JamAlignmentOption> options)
        {
            var result = new Dictionary<string, JamAlignmentOption?>(StringComparer.OrdinalIgnoreCase);
            result.Add(neitherTitle, null);

            foreach (var option in options)
            {
                result.Add(option.Title, option);
                result.Add(option.ShortTitle, option);
            }
            return result;
        }

        public IEnumerable<JamAlignmentOption> GetAvailableOptions()
            => AvailableOptions;

        public JamAlignmentOption? GetAlignment(string? name)
        {
            if (name == null)
                return null;

            if (!Lookup.ContainsKey(name))
                throw new InvalidOperationException($"Cannot resolve alignment for name '{name}'.");

            return Lookup[name];
        }
    }
}
