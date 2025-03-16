using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result
{
    public class JamTallyNewAwardScore
    {
        public JamEntry Entry { get; init; } = default!;
        public JamAwardCriterion Criterion { get; init; } = default!;
        public int Score { get; init; } = default!;

        public override string ToString()
            => $"{Criterion.Name}: {Score}";
    }
}
