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
        public IReadOnlyList<JamEntry> Authored { get; init; } = default!;
        public IReadOnlyList<JamEntry> Unjudged { get; init; } = default!;
        public IReadOnlyList<JamEntry> Missing { get; init; } = default!;

        public IReadOnlyDictionary<JamAwardCriterion, JamEntry> Awards { get; init; } = default!;

        public IReadOnlyCollection<JamVoteReaction> Reactions { get; init; } = default!;
        public int ReviewScore { get; init; }

        public static JamTallyVote ForMainTally(JamVote originalVote)
        {
            return new JamTallyVote
            {
                Voter = originalVote.Voter,
                VoterCode = ExtractVoterCode(originalVote.Voter),
                VoterAlignment = originalVote.Alignment,
                Ranking = originalVote.Ranking.ToList(),
                Authored = originalVote.Authored.ToList(),
                Unjudged = originalVote.Unjudged.ToList(),
                Missing = originalVote.Missing.ToList(),
                Awards = originalVote.Awards.ToDictionary(award => award.Criterion, award => award.Entry),
                Reactions = originalVote.AggregateReactions.OrderBy(reaction => reaction.User).ToList(),
                ReviewScore = originalVote.AggregateReactions.Sum(reaction => reaction.Value),
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
