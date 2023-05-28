namespace Alphicsh.JamTools.Common.IO.Jam
{
    public class JamAwardInfo
    {
        public string Id { get; init; } = default!;
        public string Name { get; init; } = default!;
        public string? Description { get; init; } = default!;

        public string FixedName => Name ?? Description!;
        public string? FixedDescription => Name != null ? Description : null;
    }
}
