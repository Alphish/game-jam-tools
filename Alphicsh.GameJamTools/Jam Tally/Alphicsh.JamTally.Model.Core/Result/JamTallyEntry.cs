using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result
{
    public class JamTallyEntry
    {
        public JamEntry Entry { get; init; } = default!;

        public string EntryId => Entry.Id;
        public string EntryLine => Entry.Line;
        public string Code => Entry.TallyCode;
        public string Title => Entry.TallyTitle;
        public string Authors => Entry.TallyAuthors;

        public int TotalVotesCount { get; init; }
        public int JudgedCount { get; init; }
        public int UnjudgedCount { get; init; }

        public decimal BaseScore { get; init; }
        public decimal TotalScore { get; init; }

        public IReadOnlyCollection<JamTallyNewAwardScore> AwardScores { get; init; } = default!;

        public int Rank { get; set; }
        public IList<JamAwardCriterion> Awards { get; init; } = new List<JamAwardCriterion>();

        public override string ToString()
            => $"{EntryLine}: {TotalScore:F3} ({BaseScore:F3} over {JudgedCount}/{TotalVotesCount})";
    }
}
