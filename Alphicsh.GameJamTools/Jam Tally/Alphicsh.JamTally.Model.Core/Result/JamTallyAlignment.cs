using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result
{
    public class JamTallyAlignment
    {
        public JamAlignmentOption Alignment { get; init; } = default!;
        public decimal BaseScore { get; init; }
        public int ReviewsCount { get; init; }
        public bool GuessedTheme { get; init; }
        public decimal ScoreMultiplier { get; init; }
        public decimal TotalScore { get; init; }

        public override string ToString()
            => $"{Alignment.Title}: {TotalScore:F3} ({BaseScore:F3} * {ScoreMultiplier:F2}, {ReviewsCount} reviews)";
    }
}
