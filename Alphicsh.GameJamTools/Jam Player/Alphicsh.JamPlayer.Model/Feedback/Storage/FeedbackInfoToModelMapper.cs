using System;
using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamPlayer.IO.Feedback;
using Alphicsh.JamPlayer.Model.Awards;
using Alphicsh.JamPlayer.Model.Jam;
using Alphicsh.JamPlayer.Model.Ranking;
using Alphicsh.JamPlayer.Model.Ratings;
using Alphicsh.JamTools.Common.IO.Storage;

namespace Alphicsh.JamPlayer.Model.Feedback.Loading
{
    internal class FeedbackInfoToModelMapper : IMapper<FeedbackInfo, JamFeedback>
    {
        private JamOverview Jam { get; }
        private RatingCriteriaOverview RatingCriteria { get; }

        public FeedbackInfoToModelMapper(JamOverview jam, RatingCriteriaOverview ratingCriteria)
        {
            Jam = jam;
            RatingCriteria = ratingCriteria;
        }

        public JamFeedback Map(FeedbackInfo info)
        {
            return new JamFeedback
            {
                Location = info.Location,
                Ranking = MapInfoToRanking(info),
                Awards = MapInfoToAwards(info.Awards),
            };
        }

        // -------
        // Ranking
        // -------

        private RankingOverview MapInfoToRanking(FeedbackInfo feedbackInfo)
        {
            var feedbackByEntryId = feedbackInfo.Entries.ToDictionary(entry => entry.EntryId, StringComparer.OrdinalIgnoreCase);

            Dictionary<string, RankingEntry> rankingEntriesById = Jam.Entries
                .Select(entry => MapInfoToEntry(entry, feedbackByEntryId.GetValueOrDefault(entry.Id)))
                .ToDictionary(rankingEntry => rankingEntry.JamEntry.Id);

            var rankedIds = feedbackInfo.Ranking.RankedEntries
                .Distinct()
                .ToList();

            var unrankedIds = feedbackInfo.Ranking.UnrankedEntries
                .Except(rankedIds, StringComparer.OrdinalIgnoreCase)
                .Distinct()
                .ToList();

            var pendingIds = Jam.Entries
                .Select(entry => entry.Id)
                .Except(rankedIds, StringComparer.OrdinalIgnoreCase)
                .Except(unrankedIds, StringComparer.OrdinalIgnoreCase)
                .ToList();

            return new RankingOverview
            {
                RankedEntries = rankedIds.Select(entryId => rankingEntriesById[entryId]).ToList(),
                UnrankedEntries = unrankedIds.Select(entryId => rankingEntriesById[entryId]).ToList(),
                PendingEntries = pendingIds.Select(entryId => rankingEntriesById[entryId]).ToList(),
            };
        }

        private RankingEntry MapInfoToEntry(JamEntry jamEntry, FeedbackEntryInfo? entryFeedbackInfo)
        {
            // building the list of entries
            var ratingsById = RatingCriteria.Criteria
                .Select(criterion => criterion.CreateRating())
                .ToDictionary(rating => rating.Id, StringComparer.OrdinalIgnoreCase);

            // applying the rating values stored for the entry
            var infoRatings = entryFeedbackInfo?.Ratings ?? Enumerable.Empty<FeedbackRatingInfo>();
            foreach (var ratingsInfoEntry in infoRatings)
            {
                if (!ratingsById.TryGetValue(ratingsInfoEntry.Id, out var rating))
                    continue;

                rating.Value = ratingsInfoEntry.Value;
            }

            // building and returning the ranking entry
            return new RankingEntry
            {
                JamEntry = jamEntry,
                Ratings = ratingsById.Values.ToList(),
                Comment = entryFeedbackInfo?.Comment ?? string.Empty,
                IsUnjudged = entryFeedbackInfo?.IsUnjudged == true,
            };
        }

        // ------
        // Awards
        // ------

        private AwardsOverview MapInfoToAwards(FeedbackAwardsInfo awardsInfo)
        {
            return new AwardsOverview
            {
                Entries = MapAwards(awardsInfo).ToList(),
                BestReviewer = awardsInfo.BestReviewer,
            };
        }

        private IEnumerable<AwardEntry> MapAwards(FeedbackAwardsInfo awardsInfo)
        {
            var nominationsByAwardId = awardsInfo.Nominations
                .ToDictionary(nomination => nomination.AwardId, StringComparer.OrdinalIgnoreCase);

            foreach (var criterion in Jam.AwardCriteria)
            {
                var entryId = nominationsByAwardId.GetValueOrDefault(criterion.Id)?.EntryId;
                var entry = Jam.GetEntryById(entryId);
                yield return new AwardEntry { Criterion = criterion, JamEntry = entry };
            }
        }
    }
}
