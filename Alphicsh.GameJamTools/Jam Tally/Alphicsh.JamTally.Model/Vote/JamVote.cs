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
        }

        public string Voter { get; set; } = string.Empty;
        public JamAlignmentOption? Alignment { get; set; }

        public override string ToString()
        {
            return $"vote by {Voter ?? ("<unknown voter>")}";
        }

        // -------
        // Entries
        // -------

        public IList<JamEntry> Authored { get; internal set; } = new List<JamEntry>();
        public IList<JamEntry> Ranking { get; internal set; } = new List<JamEntry>();
        public IList<JamEntry> Unjudged { get; internal set; } = new List<JamEntry>();
        public IList<JamEntry> Missing { get; internal set; } = new List<JamEntry>();

        // ------
        // Awards
        // ------

        public IReadOnlyCollection<JamVoteAward> Awards { get; internal set; } = new List<JamVoteAward>();
        public JamEntry? GetEntryForAward(JamAwardCriterion criterion)
            => Awards.FirstOrDefault(voteAward => voteAward.Criterion == criterion)?.Entry;

        public void SetEntryForAward(JamAwardCriterion criterion, JamEntry? entry)
        {
            var newAwards = Awards.Where(award => award.Criterion != criterion).ToList();
            if (entry != null)
                newAwards.Add(new JamVoteAward { Criterion = criterion, Entry = entry });

            Awards = newAwards;
        }

        // -------
        // Reviews
        // -------

        public int? DirectReviewsCount { get; internal set; }
        public bool HasDirectReviewsCount => DirectReviewsCount.HasValue && DirectReviewsCount > 0;
        public IList<JamEntry> ReviewedEntries { get; internal set; } = new List<JamEntry>();
        public int ReviewsCount => DirectReviewsCount ?? ReviewedEntries.Count;

        public void SetReviewsCountByString(string countString)
        {
            if (int.TryParse(countString, out var count))
                DirectReviewsCount = count;
            else
                DirectReviewsCount = null;
        }

        // ---------
        // Reactions
        // ---------

        public IReadOnlyCollection<JamVoteReaction> Reactions { get; internal set; } = new List<JamVoteReaction>();
        public IReadOnlyCollection<JamVoteReaction> AggregateReactions { get; internal set; } = new List<JamVoteReaction>();
        public int GetReactionScore()
            => AggregateReactions.Sum(reaction => reaction.Value);

        public string? Error { get; internal set; }

        // ----------
        // Operations
        // ----------

        public void UpdateRankableEntries(
            IReadOnlyCollection<JamEntry> allEntries,
            IReadOnlyCollection<JamEntry> authoredEntries,
            IReadOnlyCollection<JamEntry> rankingEntries,
            IReadOnlyCollection<JamEntry> unjudgedEntries
            )
        {
            Authored.Clear();
            foreach (var entry in authoredEntries)
                Authored.Add(entry);

            Ranking.Clear();
            foreach (var entry in rankingEntries)
                Ranking.Add(entry);

            Unjudged.Clear();
            foreach (var entry in unjudgedEntries)
                Unjudged.Add(entry);

            RecalculateMissingEntries(allEntries);
        }

        public void SetAuthored(IEnumerable<JamEntry> entries)
        {
            Authored.Clear();
            foreach (var entry in entries)
            {
                Authored.Add(entry);
                Ranking.Remove(entry);
                Unjudged.Remove(entry);
            }
        }

        internal void RecalculateMissingEntries(IEnumerable<JamEntry> allEntries)
        {
            Missing.Clear();
            var missingEntries = allEntries
                .Except(Authored).Except(Ranking).Except(Unjudged)
                .OrderBy(entry => entry.FullLine)
                .ToList();

            foreach (var entry in missingEntries)
                Missing.Add(entry);
        }

        public void SetReviewedEntries(IEnumerable<JamEntry> entries)
        {
            ReviewedEntries.Clear();
            foreach (var entry in entries)
                ReviewedEntries.Add(entry);
        }
    }
}
