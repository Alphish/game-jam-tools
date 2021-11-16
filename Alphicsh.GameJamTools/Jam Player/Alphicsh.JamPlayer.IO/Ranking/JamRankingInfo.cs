using System.Collections.Generic;

using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPlayer.IO.Ranking
{
    public class JamRankingInfo
    {
        public IReadOnlyCollection<string> RankedEntries { get; init; } = default!;
        public IReadOnlyCollection<string> UnrankedEntries { get; init; } = default!;
        public IReadOnlyCollection<EntryRatingsInfo> EntryRatings { get; init; } = default!;

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
            var defaultInfo = new JamRankingInfo
            {
                RankedEntries = new List<string>(),
                UnrankedEntries = new List<string>(),
                EntryRatings = new List<EntryRatingsInfo>(),
            };
            return defaultInfo;
        }

        public void SaveTo(FilePath rankingInfoPath)
        {
            var writer = new JamRankingFilesWriter();
            writer.SaveRankingInfo(this, rankingInfoPath);
        }
    }
}
