using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result
{
    public class JamTallyVote
    {
        public string Voter { get; init; } = default!;
        public JamAlignmentOption? VoterAlignment { get; init; }

        public IReadOnlyList<JamEntry> Ranking { get; init; } = default!;
        public IReadOnlyCollection<JamEntry> Unjudged { get; init; } = default!;
        public IReadOnlyDictionary<JamAwardCriterion, JamEntry> Awards { get; init; } = default!;

        public IReadOnlyCollection<JamVoteReaction> Reactions { get; init; } = default!;
        public int ReviewScore { get; init; }
    }
}
