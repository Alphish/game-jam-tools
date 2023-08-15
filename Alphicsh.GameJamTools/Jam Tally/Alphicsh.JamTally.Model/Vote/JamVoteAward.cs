using System;
using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Vote
{
    public class JamVoteAward
    {
        public JamAwardCriterion Award { get; init; } = default!;
        public JamEntry Entry { get; init; } = default!;

        public override bool Equals(object? obj)
        {
            return obj is JamVoteAward award &&
                EqualityComparer<JamAwardCriterion>.Default.Equals(Award, award.Award) &&
                EqualityComparer<JamEntry>.Default.Equals(Entry, award.Entry);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Award, Entry);
        }
    }
}
