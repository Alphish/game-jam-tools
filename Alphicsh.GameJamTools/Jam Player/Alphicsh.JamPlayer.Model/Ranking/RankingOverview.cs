using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Alphicsh.JamPlayer.Model.Jam;

namespace Alphicsh.JamPlayer.Model.Ranking
{
    public sealed class RankingOverview
    {
        public IList<RankingEntry> PendingEntries { get; set; }
        public IList<RankingEntry> RankedEntries { get; set; }
        public IList<RankingEntry> UnrankedEntries { get; set; }

        public RankingOverview()
        {
            PendingEntries = new List<RankingEntry>();
            RankedEntries = new List<RankingEntry>();
            UnrankedEntries = new List<RankingEntry>();
        }

        public IEnumerable<RankingEntry> GetAllEntries()
        {
            return PendingEntries.Concat(RankedEntries).Concat(UnrankedEntries);
        }

        public RankingEntry? GetNextEntry()
        {
            if (!PendingEntries.Any())
                return null;

            var index = RandomNumberGenerator.GetInt32(fromInclusive: 0, toExclusive: PendingEntries.Count);
            var entry = PendingEntries[index];

            PendingEntries.Remove(entry);
            UnrankedEntries.Add(entry);
            return entry;
        }

        public RankingEntry? PickEntry(JamEntry entry)
        {
            var missingEntry = PendingEntries.FirstOrDefault(rankingEntry => rankingEntry.JamEntry == entry);
            if (missingEntry == null)
            {
                return UnrankedEntries.FirstOrDefault(rankingEntry => rankingEntry.JamEntry == entry)
                    ?? RankedEntries.FirstOrDefault(rankingEntry => rankingEntry.JamEntry == entry);
            }

            PendingEntries.Remove(missingEntry);
            UnrankedEntries.Add(missingEntry);
            return missingEntry;
        }
    }
}
