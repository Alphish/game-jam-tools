namespace Alphicsh.JamTools.Common.IO.Jam.Files
{
    public class JamLauncherInfo
    {
        public string Name { get; init; } = default!;
        public string? Description { get; init; }
        public int Type { get; init; }
        public string Location { get; init; } = default!;
    }
}
