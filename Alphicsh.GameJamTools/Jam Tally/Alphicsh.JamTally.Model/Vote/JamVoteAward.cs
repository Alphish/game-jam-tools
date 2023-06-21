using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Vote
{
    public class JamVoteAward
    {
        public JamAwardCriterion Award { get; init; } = default!;
        public JamEntry Entry { get; init; } = default!;
    }
}
