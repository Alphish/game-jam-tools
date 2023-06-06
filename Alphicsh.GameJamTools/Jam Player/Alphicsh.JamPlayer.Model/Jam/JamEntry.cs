using Alphicsh.JamPlayer.Model.Jam.Files;

namespace Alphicsh.JamPlayer.Model.Jam
{
    public sealed class JamEntry
    {
        public string Id { get; init; } = default!;
        public string Title { get; init; } = default!;
        public string ShortTitle { get; init; } = default!;
        public JamTeam Team { get; init; } = default!;
        public JamFiles Files { get; init; } = default!;
    }
}
