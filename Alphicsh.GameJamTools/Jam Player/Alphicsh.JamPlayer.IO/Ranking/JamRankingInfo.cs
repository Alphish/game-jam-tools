using System;
using System.Collections.Generic;
using System.Linq;

using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPlayer.IO.Ranking
{
    public class JamRankingInfo
    {
        public IReadOnlyCollection<EntryRatingsInfo> EntryRatings { get; init; } = new List<EntryRatingsInfo>();
        public IReadOnlyCollection<string> RankedEntries { get; init; } = new List<string>();
        public IReadOnlyCollection<string> UnrankedEntries { get; init; } = new List<string>();

        private IReadOnlyDictionary<string, string?> _awards = new Dictionary<string, string?>();
        public IReadOnlyDictionary<string, string?> Awards
        {
            get => _awards;
            init => _awards = value.ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);
        }

        public string? GetAwardEntryId(string awardId) => _awards.TryGetValue(awardId, out var entryId) ? entryId : null;

        // ---------------
        // Reading/writing
        // ---------------

        public static JamRankingInfo LoadOrGetDefault(FilePath rankingInfoPath)
        {
            var reader = new JamRankingFilesReader();

            // try read ranking info from file
            var infoFromFile = reader.TryLoadRankingInfo(rankingInfoPath);
            if (infoFromFile != null)
                return infoFromFile;

            // if ranking info is not available, return the default one
            var defaultInfo = new JamRankingInfo();
            return defaultInfo;
        }

        public void SaveTo(FilePath rankingInfoPath)
        {
            var writer = new JamRankingFilesWriter();
            writer.SaveRankingInfo(this, rankingInfoPath);
        }
    }
}
