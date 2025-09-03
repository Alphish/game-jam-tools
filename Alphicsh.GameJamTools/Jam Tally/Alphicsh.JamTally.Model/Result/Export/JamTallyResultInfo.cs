using System.Collections.Generic;

namespace Alphicsh.JamTally.Model.Result.Export
{
    public class JamTallyResultInfo
    {
        public IReadOnlyCollection<string> Ranking { get; init; } = default!;
        public IReadOnlyCollection<JamTallyAwardInfo> Awards { get; init; } = default!;
    }
}
