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
        // -----------------
        // Loading from info
        // -----------------

        public RankingOverview MapInfoToRanking(JamRankingInfo rankingInfo, JamOverview jam, RatingCriteriaOverview criteria)
        {
            var entriesDictionary = jam.Entries.ToDictionary(entry => entry.Id, StringComparer.OrdinalIgnoreCase);
            var ratingsDictionary = rankingInfo.EntryRatings.ToDictionary(entry => entry.EntryId, StringComparer.OrdinalIgnoreCase);
            var blankRatingsDictionary = new Dictionary<string, EntryRatingsInfo>();

            var pendingIds = jam.Entries
                .Select(entry => entry.Id)
                .Except(rankingInfo.RankedEntries, StringComparer.OrdinalIgnoreCase)
                .Except(rankingInfo.UnrankedEntries, StringComparer.OrdinalIgnoreCase)
                .ToList();
            var pendingEntries = MapInfoListToEntries(pendingIds, entriesDictionary, blankRatingsDictionary, criteria).ToList();

            var rankedEntries = MapInfoListToEntries(rankingInfo.RankedEntries, entriesDictionary, ratingsDictionary, criteria).ToList();
            var unrankedEntries = MapInfoListToEntries(rankingInfo.UnrankedEntries, entriesDictionary, ratingsDictionary, criteria).ToList();

            return new RankingOverview
            {
                PendingEntries = pendingEntries,
                RankedEntries = rankedEntries,
                UnrankedEntries = unrankedEntries,
            };
        }

        private IEnumerable<RankingEntry> MapInfoListToEntries(
            IReadOnlyCollection<string> entryIds,
            IDictionary<string, JamEntry> entriesDictionary,
            IDictionary<string, EntryRatingsInfo> ratingsDictionary,
            RatingCriteriaOverview criteria
            )
        {
            foreach (var entryId in entryIds)
            {
                if (!entriesDictionary.TryGetValue(entryId, out var jamEntry))
                    continue;

                var ratingsInfo = ratingsDictionary.TryGetValue(entryId, out var foundRatingsInfo) ? foundRatingsInfo : null;

                yield return MapInfoToEntry(jamEntry, ratingsInfo, criteria);
            }
        }

        private RankingEntry MapInfoToEntry(JamEntry jamEntry, EntryRatingsInfo? ratingsInfo, RatingCriteriaOverview criteria)
        {
            // building the list of entries
            var ratings = criteria.Criteria.Select(criterion => criterion.CreateRating()).ToList();
            var ratingsDictionary = ratings.ToDictionary(rating => rating.Id, StringComparer.OrdinalIgnoreCase);

            // applying the rating values stored for the entry
            var infoRatings = ratingsInfo?.Ratings ?? Enumerable.Empty<RatingInfo>();
            foreach (var ratingsInfoEntry in infoRatings)
            {
                if (!ratingsDictionary.TryGetValue(ratingsInfoEntry.Id, out var rating))
                    continue;

                rating.Value = ratingsInfoEntry.Value;
            }

            // building and returning the ranking entry
            return new RankingEntry
            {
                JamEntry = jamEntry,
                Ratings = ratings,
                Comment = ratingsInfo?.Comment ?? string.Empty,
            };
        }

        public AwardsOverview MapInfoToAwards(JamRankingInfo rankingInfo, JamOverview jam)
        {
            return new AwardsOverview
            {
                Entries = MapAwards(rankingInfo, jam).ToList(),
            };
        }

        private IEnumerable<AwardEntry> MapAwards(JamRankingInfo rankingInfo, JamOverview jam)
        {
            foreach (var criterion in jam.AwardCriteria)
            {
                var entryId = rankingInfo.GetAwardEntryId(criterion.Id);
                var entry = jam.GetEntryById(entryId);
                yield return new AwardEntry { Criterion = criterion, JamEntry = entry };
            }
        }

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
