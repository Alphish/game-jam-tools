using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamPlayer.Model.Jam;
using Alphicsh.JamPlayer.Model.Ratings;

namespace Alphicsh.JamPlayer.Model.Ranking
{
    public sealed class RankingEntry
    {
        public JamEntry JamEntry { get; init; } = default!;
        public string Comment { get; set; } = string.Empty;
        public int? Rank { get; set; } = default!;
        public bool IsUnjudged { get; set; }

        private IReadOnlyCollection<IRating> InnerRatings { get; init; } = default!;
        private IReadOnlyDictionary<string, IRating> RatingsById { get; init; } = default!;
        public IReadOnlyCollection<IRating> Ratings
        {
            get => InnerRatings;
            init
            {
                InnerRatings = value.ToList();
                RatingsById = InnerRatings.ToDictionary(rating => rating.Id);
            }
        }

        public bool HasFeedback => Ratings.Any() || !string.IsNullOrEmpty(Comment) || IsUnjudged;

        public object? GetProperty(string propertyName)
        {
            propertyName = propertyName.ToLowerInvariant();
            switch (propertyName)
            {
                case "title":
                    return JamEntry.Title;
                case "team":
                    return JamEntry.Team.Description;
                case "authors":
                    return JamEntry.Team.AuthorNames;
                case "comment":
                    return Comment;
                case "rank":
                    return Rank;
                default:
                    return RatingsById.TryGetValue(propertyName, out var rating) ? rating : null;
            }
        }
    }
}
