using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result
{
    public class JamTallyEntryScore
    {
        public JamEntry Entry { get; init; } = default!;
        public int TotalVotesCount { get; init; }

        public int UnjudgedCount { get; private set; } = 0;
        public decimal BaseScore { get; private set; } = 0;

        public int JudgedCount => TotalVotesCount - UnjudgedCount;
        public decimal TotalScore => BaseScore * TotalVotesCount / JudgedCount;

        internal void AddRank(int rank)
        {
            BaseScore += 1m / (rank + 1);
        }

        internal void MarkUnjudged()
        {
            UnjudgedCount++;
        }

        public override string ToString()
            => $"{Entry}: {TotalScore} ({BaseScore} over {JudgedCount}/{TotalVotesCount})";
    }
}
