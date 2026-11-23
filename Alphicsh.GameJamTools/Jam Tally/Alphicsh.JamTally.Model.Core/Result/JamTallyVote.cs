using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result
{
    public class JamTallyVote
    {
        public string Voter { get; init; } = default!;
        public string VoterCode { get; init; } = default!;
        public JamAlignmentOption? VoterAlignment { get; init; }

        public IReadOnlyList<JamEntry> Ranking { get; init; } = default!;
        public IReadOnlyList<JamEntry> Unjudged { get; init; } = default!;
        public IReadOnlyList<JamEntry> Missing { get; init; } = default!;

        public IReadOnlyDictionary<JamAwardCriterion, JamEntry> Awards { get; init; } = default!;

        public IReadOnlyCollection<JamVoteReaction> Reactions { get; init; } = default!;
        public int ReviewScore { get; init; }
        public bool IsComplete { get; init; }

        public static JamTallyVote ForMainTally(JamVote originalVote)
        {
            return new JamTallyVote
            {
                Voter = originalVote.Voter,
                VoterCode = ExtractVoterCode(originalVote.Voter),
                VoterAlignment = originalVote.Alignment,
                Ranking = originalVote.Ranking.ToList(),
                Unjudged = originalVote.Authored.Concat(originalVote.Unjudged.OrderBy(entry => entry.FullLine)).Distinct().ToList(),
                Missing = originalVote.Missing.ToList(),
                Awards = originalVote.Awards.ToDictionary(award => award.Criterion, award => award.Entry),
                Reactions = originalVote.AggregateReactions.OrderBy(reaction => reaction.User).ToList(),
                ReviewScore = originalVote.AggregateReactions.Sum(reaction => reaction.Value),
                IsComplete = originalVote.ReviewsCount > 0,
            };
        }

        public static JamTallyVote ForAlignmentTally(JamVote originalVote)
        {
            Func<JamEntry, bool> entryHasAlignment = entry => entry.Alignment != null;
            Func<JamEntry, bool> entryIsVoterAligned = entry => originalVote.Alignment != null && entry.Alignment == originalVote!.Alignment;
            Func<JamEntry, bool> entryIsCrossAligned = entry => entry.Alignment != null && entry.Alignment != originalVote.Alignment;

            var authored = originalVote.Authored.Where(entryHasAlignment).ToList();
            var unjudged = originalVote.Unjudged.Where(entryHasAlignment) // remove non-aligned entries altogether
                .Concat(originalVote.Ranking.Where(entryIsVoterAligned)) // include voter-aligned ranking entries as inherently unjudgable
                .Concat(originalVote.Missing.Where(entryIsVoterAligned)) // include voter-aligned missing-entries as inherently unjudgable
                .ToList();

            return new JamTallyVote
            {
                Voter = originalVote.Voter,
                VoterCode = ExtractVoterCode(originalVote.Voter),
                VoterAlignment = originalVote.Alignment,
                Ranking = originalVote.Ranking.Where(entryIsCrossAligned).ToList(),
                Unjudged = authored.Concat(unjudged.OrderBy(entry => entry.FullLine)).Distinct().ToList(),
                Missing = originalVote.Missing.Where(entryIsCrossAligned).ToList(),
                Awards = new Dictionary<JamAwardCriterion, JamEntry>(), // awards are irrelevant for alignments, so an empty collection is passed
                IsComplete = originalVote.ReviewsCount > 0,
            };
        }

        private static string ExtractVoterCode(string title)
        {
            var codeBuilder = new StringBuilder();
            foreach (char c in title)
            {
                if (c >= 128)
                    continue;

                if (char.IsLetterOrDigit(c))
                    codeBuilder.Append(c);
                else if (codeBuilder.Length > 0 && codeBuilder[^1] != '_')
                    codeBuilder.Append('_');
            }
            return codeBuilder.ToString().ToLowerInvariant();
        }
    }
}
