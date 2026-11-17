using System;
using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Vote
{
    public class JamVoteAward
    {
        public JamAwardCriterion Criterion { get; init; } = default!;
        public JamEntry Entry { get; init; } = default!;

        public override bool Equals(object? obj)
        {
            return obj is JamVoteAward award &&
                EqualityComparer<JamAwardCriterion>.Default.Equals(Criterion, award.Criterion) &&
                EqualityComparer<JamEntry>.Default.Equals(Entry, award.Entry);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Criterion, Entry);
        }

        public string FullLine => $"{Criterion.Name}: {Entry.FullLine}";
        public override string ToString() => FullLine;
    }
}
