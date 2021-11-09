using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPlayer.Model.Jam
{
    public sealed class JamEntry
    {
        public string Title { get; init; } = default!;
        public JamTeam Team { get; init; } = default!;

        public FilePath? GamePath { get; init; } = default!;
        public FilePath? ThumbnailPath { get; init; } = default!;
        public FilePath? ThumbnailSmallPath { get; init; } = default!;
    }
}
