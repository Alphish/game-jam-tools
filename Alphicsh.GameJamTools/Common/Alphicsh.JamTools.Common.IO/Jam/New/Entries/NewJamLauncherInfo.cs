namespace Alphicsh.JamTools.Common.IO.Jam.New.Entries
{
    public class NewJamLauncherInfo
    {
        public string Name { get; init; } = default!;
        public string? Description { get; init; }
        public int Type { get; init; }
        public string Location { get; init; } = default!;
    }
}
