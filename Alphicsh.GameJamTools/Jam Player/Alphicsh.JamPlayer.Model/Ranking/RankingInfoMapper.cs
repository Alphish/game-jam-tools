using System.Linq;

using Alphicsh.JamPlayer.IO.Ranking;

namespace Alphicsh.JamPlayer.Model.Ranking
{
    public class RankingInfoMapper
    {
        // --------------
        // Saving to info
        // --------------

        public JamRankingInfo MapRankingToInfo(RankingOverview ranking)
        {
            var entryRatings = ranking.GetAllEntries()
                .Where(RankingEntryHasRatings)
                .Select(MapRankingEntryToRatingsInfo)
                .OrderBy(info => info!.EntryId)
                .ToList();

            var rankedEntries = ranking.RankedEntries.Select(entry => entry.JamEntry.Id).ToList();
            var uunrankedEntries = ranking.UnrankedEntries.Select(entry => entry.JamEntry.Id).ToList();

            return new JamRankingInfo
            {
                EntryRatings = entryRatings,
                RankedEntries = rankedEntries,
                UnrankedEntries = uunrankedEntries,
            };
        }

        private bool RankingEntryHasRatings(RankingEntry entry)
        {
            return entry.Ratings.Any(rating => rating.HasValue) || !string.IsNullOrWhiteSpace(entry.Comment);
        }

        private EntryRatingsInfo MapRankingEntryToRatingsInfo(RankingEntry entry)
        {
            var ratings = entry.Ratings
                .Where(rating => rating.HasValue)
                .Select(rating => new RatingInfo { Id = rating.Id, Value = rating.Value })
                .ToList();

            var comment = !string.IsNullOrWhiteSpace(entry.Comment) ? entry.Comment : null;

            return new EntryRatingsInfo
            {
                EntryId = entry.JamEntry.Id,
                Ratings = ratings,
                Comment = comment,
            };
        }
    }
}
