using System.Collections.Generic;

namespace Alphicsh.JamPlayer.Model.Ranking
{
    public sealed class RankingOverview
    {
        public IList<RankingEntry> RankedEntries { get; set; }
        public IList<RankingEntry> UnrankedEntries { get; set; }

        public RankingEntry SelectedEntry { get; set; }

        public RankingOverview()
        {
            RankedEntries = new List<RankingEntry>();
            UnrankedEntries = new List<RankingEntry>();
        }
    }
}
