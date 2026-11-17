using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result
{
    public class JamTallyResult
    {
        public IReadOnlyList<JamTallyEntry> Ranking { get; init; } = default!;
        public JamTallyAlignmentResult? AlignmentResult { get; init; } = default!;

        public IReadOnlyList<JamAwardCriterion> AwardCriteria { get; init; } = default!;
        public IReadOnlyDictionary<JamAwardCriterion, IReadOnlyList<JamTallyAwardScore>> AwardEntryScores { get; init; } = default!;
        public IReadOnlyDictionary<JamAwardCriterion, int> AwardTopScores { get; init; } = default!;
        public IReadOnlyDictionary<JamAwardCriterion, IReadOnlyList<JamEntry>> AwardWinners { get; init; } = default!;

        public IReadOnlyCollection<JamTallyVote> Votes { get; init; } = default!;
        public int TopReviewScore { get; init; }
        public IReadOnlyCollection<string> TopReviewers { get; init; } = default!;
    }
}
