using System;
using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamPlayer.Model.Ranking;

namespace Alphicsh.JamPlayer.Model.Export
{
    using ValueSelector = Func<RankingEntry, string>;

    internal sealed class EntryTemplate
    {
        private string Format { get; }
        private IReadOnlyCollection<ValueSelector> ValueSelectors { get; }

        public EntryTemplate(string format, IEnumerable<ValueSelector> valueSelectors)
        {
            Format = format;
            ValueSelectors = valueSelectors.ToList();
        }

        public string FormatEntry(RankingEntry rankingEntry)
        {
            var values = ValueSelectors.Select(selector => selector(rankingEntry)).ToArray();
            return string.Format(Format, values);
        }
    }
}
