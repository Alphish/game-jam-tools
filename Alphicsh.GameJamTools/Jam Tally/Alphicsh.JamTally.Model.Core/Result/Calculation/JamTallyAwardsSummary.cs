using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Calculation
{
    internal class JamTallyAwardsSummary
    {
        public IReadOnlyList<JamAwardCriterion> AwardCriteria { get; init; } = default!;
        public IReadOnlyDictionary<JamAwardCriterion, IReadOnlyList<JamTallyAwardScore>> AwardEntryScores { get; init; } = default!;
        public IReadOnlyDictionary<JamAwardCriterion, int> AwardTopScores { get; init; } = default!;
        public IReadOnlyDictionary<JamAwardCriterion, IReadOnlyList<JamEntry>> AwardWinners { get; init; } = default!;
    }
}
