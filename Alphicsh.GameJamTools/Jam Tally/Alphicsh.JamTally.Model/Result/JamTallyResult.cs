using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result
{
    public class JamTallyResult
    {
        public IReadOnlyCollection<JamAwardCriterion> Awards { get; init; } = default!;
        public IReadOnlyCollection<JamEntry> Entries { get; init; } = default!;
        public IReadOnlyCollection<JamVote> Votes { get; init; } = default!;

        public IReadOnlyCollection<JamTallyEntryScore> FinalRanking { get; init; } = default!;
        public IReadOnlyDictionary<JamAwardCriterion, JamTallyAwardRanking> AwardRankings { get; init; } = default!;
    }
}
