using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamPlayer.Model.Jam
{
    public sealed class JamTeam
    {
        public string? Name { get; init; } = default!;
        public IReadOnlyCollection<JamAuthor> Authors { get; init; } = default!;

        public string Description => Name ?? AuthorNames;
        public string AuthorNames => string.Join(", ", Authors.Select(author => author.Name));
    }
}
