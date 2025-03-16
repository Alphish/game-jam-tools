using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result
{
    public class JamTallyNewResult
    {
        public IReadOnlyCollection<JamTallyEntry> Ranking { get; init; } = default!;
        public IReadOnlyCollection<JamAwardCriterion> AwardCriteria { get; init; } = default!;
        public IReadOnlyDictionary<JamAwardCriterion, int> AwardTopScores { get; init; } = default!;
        public IReadOnlyDictionary<JamAwardCriterion, IReadOnlyCollection<JamEntry>> AwardEntries { get; init; } = default!;

        public IReadOnlyCollection<JamTallyVote> Votes { get; init; } = default!;
        public int TopReviewScore { get; init; }
        public IReadOnlyCollection<string> TopReviewers { get; init; } = default!;
    }
}
