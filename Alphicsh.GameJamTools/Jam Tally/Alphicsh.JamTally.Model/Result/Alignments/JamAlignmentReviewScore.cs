using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Alignments
{
    public class JamAlignmentReviewScore
    {
        public JamAlignmentOption Alignment { get; init; } = default!;
        public int EligibleVotes { get; init; }
        public int VotelessEntries { get; init; }
        public decimal Multiplier => 1 + (decimal)EligibleVotes / (decimal)(EligibleVotes + VotelessEntries);
    }
}
