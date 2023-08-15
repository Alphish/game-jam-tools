using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result
{
    public class JamTallyAwardScore
    {
        public JamVoteAward VoteAward { get; init; } = default!;
        public int Count { get; init; }

        public JamAwardCriterion Award => VoteAward.Award;
        public JamEntry Entry => VoteAward.Entry;

        public override string ToString()
            => $"{Award}: {Entry} ({Count})";
    }
}
