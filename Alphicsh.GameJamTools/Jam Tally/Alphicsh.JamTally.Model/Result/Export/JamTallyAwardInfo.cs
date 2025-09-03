using System.Collections.Generic;

namespace Alphicsh.JamTally.Model.Result.Export
{
    public class JamTallyAwardInfo
    {
        public string Id { get; init; } = default!;
        public string Name { get; init; } = default!;
        public IReadOnlyCollection<string> Winners { get; init; } = default!;
        public string AwardedTo { get; init; } = default!;
    }
}
