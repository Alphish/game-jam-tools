using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote.Search;

namespace Alphicsh.JamTally.Model.Vote
{
    public class JamVote
    {
        public string Content { get; set; } = default!;

        public JamVote(string content = "")
        {
            Content = content;
        }

        public string Voter { get; set; } = string.Empty;
        public JamAlignmentOption? Alignment { get; set; }

        public IList<JamEntry> Ranking { get; internal set; } = new List<JamEntry>();
        public IList<JamEntry> Authored { get; internal set; } = new List<JamEntry>();
        public IList<JamEntry> Unjudged { get; internal set; } = new List<JamEntry>();
        public IList<JamEntry> Missing { get; internal set; } = new List<JamEntry>();

        public IReadOnlyCollection<JamVoteAward> Awards { get; internal set; } = new List<JamVoteAward>();
        public JamEntry? FindEntryForAward(JamAwardCriterion award)
            => Awards.FirstOrDefault(voteAward => voteAward.Criterion == award)?.Entry;

        public int? DirectReviewsCount { get; internal set; }
        public bool HasDirectReviewsCount => DirectReviewsCount.HasValue && DirectReviewsCount > 0;
        public IReadOnlyCollection<JamEntry> ReviewedEntries { get; internal set; } = new List<JamEntry>();
        public int ReviewsCount => DirectReviewsCount ?? ReviewedEntries.Count;

        public IReadOnlyCollection<JamVoteReaction> Reactions { get; internal set; } = new List<JamVoteReaction>();
        public IReadOnlyCollection<JamVoteReaction> AggregateReactions { get; internal set; } = new List<JamVoteReaction>();
        public int GetReactionScore()
            => AggregateReactions.Sum(reaction => reaction.Value);

        public string? Error { get; internal set; }

        public void ProcessContent()
        {
            var search = new JamSearch(JamTallyModel.Current.Jam!);
            var parser = new JamVoteContentProcessor(this, search);
            parser.Process();
        }

        public override string ToString()
        {
            return $"vote by {Voter ?? ("<unknown voter>")}";
        }
    }
}
