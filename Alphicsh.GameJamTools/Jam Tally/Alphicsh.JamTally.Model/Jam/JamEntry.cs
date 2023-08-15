using System.Collections.Generic;

namespace Alphicsh.JamTally.Model.Jam
{
    public class JamEntry
    {
        public string Id { get; init; } = default!;
        public string Title { get; init; } = default!;
        public string Team { get; init; } = default!;
        public string? RawTeam { get; init; } = default!;
        public IReadOnlyCollection<string> Authors { get; init; } = default!;

        public string Line => $"{Title} by {Team}";

        public override string ToString()
            => Line;
    }
}
