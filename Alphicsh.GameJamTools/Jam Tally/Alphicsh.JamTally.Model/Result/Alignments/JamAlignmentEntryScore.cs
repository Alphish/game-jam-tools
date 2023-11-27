using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Alignments
{
    public class JamAlignmentEntryScore
    {
        public JamEntry Entry { get; init; } = default!;
        public bool IsRanked { get; init; }
        
        public int CompetingEntriesAbove { get; init; }
        public int CompetingEntriesTotal { get; init; }
        public int CompetingEntriesBelow => CompetingEntriesTotal - CompetingEntriesAbove;

        public decimal AlignmentScore
            => (IsRanked ? 1m : 0.5m) * CompetingEntriesBelow / CompetingEntriesTotal;

        public override string ToString()
            => $"{Entry}: {AlignmentScore:P} ({CompetingEntriesBelow}/{CompetingEntriesTotal}{(IsRanked ? "" : " NR")})";
    }
}
