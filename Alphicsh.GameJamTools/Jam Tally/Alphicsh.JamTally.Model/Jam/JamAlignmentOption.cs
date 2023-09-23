namespace Alphicsh.JamTally.Model.Jam
{
    public class JamAlignmentOption
    {
        public string Title { get; init; } = default!;
        public string ShortTitle { get; init; } = default!;

        public override string ToString() => Title;
    }
}
