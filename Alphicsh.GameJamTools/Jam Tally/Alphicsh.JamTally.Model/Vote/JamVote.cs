using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Vote
{
    public class JamVote
    {
        public string Content { get; set; } = default!;

        public JamVote(string content = "")
        {
            Content = content;
            ProcessContent();
        }

        public string? Voter { get; internal set; }
        public JamAlignmentOption? Alignment { get; internal set; }

        public IReadOnlyCollection<JamEntry> Ranking { get; internal set; } = new List<JamEntry>();
        public IReadOnlyCollection<JamEntry> Authored { get; internal set; } = new List<JamEntry>();
        public IReadOnlyCollection<JamEntry> Unjudged { get; internal set; } = new List<JamEntry>();
        public IReadOnlyCollection<JamEntry> Missing { get; internal set; } = new List<JamEntry>();

        public IReadOnlyCollection<JamVoteAward> Awards { get; internal set; } = new List<JamVoteAward>();
        public JamEntry? FindEntryForAward(JamAwardCriterion award)
            => Awards.FirstOrDefault(voteAward => voteAward.Criterion == award)?.Entry;

        public int? DirectReviewsCount { get; internal set; }
        public IReadOnlyCollection<JamEntry> ReviewedEntries { get; internal set; } = new List<JamEntry>();
        public int ReviewsCount => DirectReviewsCount ?? ReviewedEntries.Count;

        public IReadOnlyCollection<JamVoteReaction> Reactions { get; internal set; } = new List<JamVoteReaction>();
        public int GetReactionScore()
            => Reactions.Sum(reaction => reaction.Value);

        public string? Error { get; internal set; }

        public void ProcessContent()
        {
            var parser = new JamVoteContentProcessor(this);
            parser.Process();
        }

        public override string ToString()
        {
            return $"vote by {Voter ?? ("<unknown voter>")}";
        }
    }
}
