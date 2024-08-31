using System.Linq;
using Alphicsh.JamPlayer.IO.Feedback;
using Alphicsh.JamPlayer.Model.Awards;
using Alphicsh.JamPlayer.Model.Ranking;
using Alphicsh.JamTools.Common.IO.Storage;

namespace Alphicsh.JamPlayer.Model.Feedback.Storage
{
    internal class FeedbackModelToInfoMapper : IMapper<JamFeedback, FeedbackInfo>
    {
        public FeedbackInfo Map(JamFeedback feedback)
        {
            var entryRatings = feedback.Ranking.GetAllEntries()
                .Where(RankingEntryHasRatings)
                .Select(MapRankingEntryToRatingsInfo)
                .OrderBy(info => info!.EntryId)
                .ToList();

            var ranking = MapRankingToInfo(feedback.Ranking);
            var awards = MapAwardsToInfo(feedback.Awards);

            return new FeedbackInfo
            {
                Location = feedback.Location,
                Entries = entryRatings,
                Ranking = ranking,
                Awards = awards,
            };
        }

        // -------
        // Ranking
        // -------

        private bool RankingEntryHasRatings(RankingEntry entry)
        {
            return entry.Ratings.Any(rating => rating.HasValue)
                || !string.IsNullOrWhiteSpace(entry.Comment)
                || entry.IsUnjudged == true;
        }

        private FeedbackEntryInfo MapRankingEntryToRatingsInfo(RankingEntry entry)
        {
            var ratings = entry.Ratings
                .Where(rating => rating.HasValue)
                .Select(rating => new FeedbackRatingInfo { Id = rating.Id, Value = rating.Value })
                .ToList();

            var comment = !string.IsNullOrWhiteSpace(entry.Comment) ? entry.Comment : null;

            return new FeedbackEntryInfo
            {
                EntryId = entry.JamEntry.Id,
                Ratings = ratings,
                Comment = comment,
                IsUnjudged = entry.IsUnjudged ? true : null,
            };
        }

        private FeedbackRankingInfo MapRankingToInfo(RankingOverview ranking)
        {
            var rankedIds = ranking.RankedEntries.Select(entry => entry.JamEntry.Id).ToList();
            var unrankedIds = ranking.UnrankedEntries.Select(entry => entry.JamEntry.Id).ToList();

            return new FeedbackRankingInfo
            {
                RankedEntries = rankedIds,
                UnrankedEntries = unrankedIds,
            };
        }

        // ------
        // Awards
        // ------

        private FeedbackAwardsInfo MapAwardsToInfo(AwardsOverview awards)
        {
            var nominations = awards.Entries
                .Where(award => award.JamEntry != null)
                .Select(award => new FeedbackAwardNominationInfo { AwardId = award.Criterion.Id, EntryId = award.JamEntry!.Id })
                .ToList();

            return new FeedbackAwardsInfo { Nominations = nominations };
        }
    }
}
