﻿using System.Collections.Generic;
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

        // -------
        // Entries
        // -------

        public IList<JamEntry> Authored { get; internal set; } = new List<JamEntry>();
        public IList<JamEntry> Ranking { get; internal set; } = new List<JamEntry>();
        public IList<JamEntry> Unjudged { get; internal set; } = new List<JamEntry>();
        public IList<JamEntry> Missing { get; internal set; } = new List<JamEntry>();

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
        public IReadOnlyCollection<JamEntry> ReviewedEntries { get; internal set; } = new List<JamEntry>();
        public int ReviewsCount => DirectReviewsCount ?? ReviewedEntries.Count;

        // ---------
        // Reactions
        // ---------

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
