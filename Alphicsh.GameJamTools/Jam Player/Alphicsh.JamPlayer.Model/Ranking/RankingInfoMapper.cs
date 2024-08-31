using System;
using System.Collections.Generic;
using System.Linq;

using Alphicsh.JamPlayer.IO.Ranking;
using Alphicsh.JamPlayer.Model.Awards;
using Alphicsh.JamPlayer.Model.Jam;
using Alphicsh.JamPlayer.Model.Ratings;

namespace Alphicsh.JamPlayer.Model.Ranking
{
    public class RankingInfoMapper
    {

        // --------------
        // Saving to info
        // --------------

        public JamRankingInfo MapRankingToInfo(RankingOverview ranking, AwardsOverview awards)
        {
            var entryRatings = ranking.GetAllEntries()
                .Where(RankingEntryHasRatings)
                .Select(MapRankingEntryToRatingsInfo)
                .OrderBy(info => info!.EntryId)
                .ToList();

            var rankedEntries = ranking.RankedEntries.Select(entry => entry.JamEntry.Id).ToList();
            var unrankedEntries = ranking.UnrankedEntries.Select(entry => entry.JamEntry.Id).ToList();
            var awardsDictionary = awards.Entries.ToDictionary(entry => entry.Criterion.Id, entry => entry.JamEntry?.Id);

            return new JamRankingInfo
            {
                EntryRatings = entryRatings,
                RankedEntries = rankedEntries,
                UnrankedEntries = unrankedEntries,
                Awards = awardsDictionary,
            };
        }

        private bool RankingEntryHasRatings(RankingEntry entry)
        {
            return entry.Ratings.Any(rating => rating.HasValue) || !string.IsNullOrWhiteSpace(entry.Comment) || entry.IsUnjudged == true;
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
                IsUnjudged = entry.IsUnjudged ? true : null,
            };
        }
    }
}
