using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamPlayer.IO.Feedback.Legacy
{
    internal class LegacyFeedbackInfo
    {
        public static readonly string Filename = "ranking.jamranking";

        public IReadOnlyCollection<LegacyFeedbackEntryInfo> EntryRatings { get; init; } = new List<LegacyFeedbackEntryInfo>();
        public IReadOnlyCollection<string> RankedEntries { get; init; } = new List<string>();
        public IReadOnlyCollection<string> UnrankedEntries { get; init; } = new List<string>();

        private IReadOnlyDictionary<string, string?> _awards = new Dictionary<string, string?>();
        public IReadOnlyDictionary<string, string?> Awards
        {
            get => _awards;
            init => _awards = value.ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);
        }

        public string? GetAwardEntryId(string awardId) => _awards.TryGetValue(awardId, out var entryId) ? entryId : null;

        // ----------
        // New format
        // ----------

        internal FeedbackInfo ToNewFormat()
        {
            return new FeedbackInfo
            {
                Entries = EntryRatings.Select(ToNewEntryFormat).ToList(),
                Ranking = new FeedbackRankingInfo
                {
                    RankedEntries = RankedEntries,
                    UnrankedEntries = UnrankedEntries,
                },
                Awards = new FeedbackAwardsInfo
                {
                    Nominations = Awards.Select(ToNewAwardNominationFormat).ToList(),
                },
            };
        }

        private FeedbackEntryInfo ToNewEntryFormat(LegacyFeedbackEntryInfo entry)
        {
            return new FeedbackEntryInfo
            {
                EntryId = entry.EntryId,
                Ratings = entry.Ratings.Select(ToNewRatingFormat).ToList(),
                Comment = entry.Comment,
                IsUnjudged = entry.IsUnjudged,
            };
        }

        private FeedbackRatingInfo ToNewRatingFormat(LegacyFeedbackRatingInfo rating)
        {
            return new FeedbackRatingInfo
            {
                Id = rating.Id,
                Value = rating.Value,
            };
        }

        private FeedbackAwardNominationInfo ToNewAwardNominationFormat(KeyValuePair<string, string?> nomination)
        {
            return new FeedbackAwardNominationInfo
            {
                AwardId = nomination.Key,
                EntryId = nomination.Value,
            };
        }
    }
}
