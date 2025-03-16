namespace Alphicsh.JamTally.Model.Jam
{
    public class JamAwardCriterion
    {
        public string Id { get; init; } = default!;
        public string Name { get; init; } = default!;

        public string TallyName { get; init; } = default!;
        public string Abbreviation { get; init; } = default!;

        public override string ToString()
            => Name;
    }
}
