using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets.Votes
{
    internal class VotesSheet : Sheet
    {
        public TallyWorkbook Workbook { get; }
        public JamTallyResult TallyResult { get; }

        public int HeaderRow { get; }
        public int LabelColumn { get; }

        public int VotesCount => TallyResult.Votes.Count;
        public int VotesFirstColumn { get; }
        public int VotesLastColumn => VotesFirstColumn + 2 * VotesCount - 1;
        public IReadOnlyDictionary<JamVote, int> VoteColumns { get; }

        public int EntriesCount => TallyResult.EntriesCount;
        public int EntriesFirstRow { get; }
        public int EntriesLastRow => EntriesFirstRow + EntriesCount - 1;

        public int AwardsCount => TallyResult.AwardsCount;
        public int AwardsFirstRow { get; }
        public int AwardsLastRow => AwardsFirstRow + AwardsCount - 1;
        public IReadOnlyDictionary<JamAwardCriterion, int> AwardRows { get; }

        public int UnjudgedCount { get; }
        public int UnjudgedFirstRow { get; }
        public int UnjudgedLastRow => UnjudgedFirstRow + UnjudgedCount - 1;

        public int ReactionsCount { get; }
        public int ReactionsFirstRow { get; }
        public int ReactionsLastRow => ReactionsFirstRow + ReactionsCount - 1;
        public int ReactionsTotalRow => ReactionsLastRow + 1;

        public int TotalWidth => VotesLastColumn;
        public int TotalHeight => ReactionsTotalRow;

        public VotesSheet(TallyWorkbook workbook)
        {
            Workbook = workbook;
            TallyResult = Workbook.TallyResult;

            HeaderRow = 1;
            LabelColumn = 1;
            
            VotesFirstColumn = 2;
            VoteColumns = MakeVoteColumns();
            
            EntriesFirstRow = 2;
            
            AwardsFirstRow = EntriesLastRow + 1;
            AwardRows = MakeAwardRows();

            UnjudgedCount = TallyResult.Votes.Max(vote => vote.Authored.Union(vote.Unjudged).Count());
            UnjudgedFirstRow = AwardsLastRow + 1;

            ReactionsCount = TallyResult.Votes.Max(vote => vote.AggregateReactions.Count);
            ReactionsFirstRow = UnjudgedLastRow + 1;

            ExpandTo(TotalWidth, TotalHeight);
        }

        private IReadOnlyDictionary<JamVote, int> MakeVoteColumns()
        {
            var result = new Dictionary<JamVote, int>();
            var column = VotesFirstColumn;
            foreach (var vote in TallyResult.Votes)
            {
                result.Add(vote, column);
                column += 2;
            }
            return result;
        }

        private IReadOnlyDictionary<JamAwardCriterion, int> MakeAwardRows()
        {
            var result = new Dictionary<JamAwardCriterion, int>();
            var row = AwardsFirstRow;
            foreach (var award in TallyResult.Awards)
            {
                result.Add(award, row);
                row++;
            }
            return result;
        }

        // ----------
        // Populating
        // ----------

        public void Populate()
        {
            var labelArea = new VotesLabelArea(this);
            labelArea.Populate();

            foreach (var vote in TallyResult.Votes)
            {
                var voteArea = new VotesPostArea(this, vote);
                voteArea.Populate();
            }
        }

        // ----------------
        // Helper functions
        // ----------------

        public string GetVoteTitleRange(JamVote vote)
        {
            var column = VoteColumns[vote];
            var cellFrom = Cell.At(column, EntriesFirstRow, absoluteColumn: false, absoluteRow: true);
            var cellTo = Cell.At(column, EntriesLastRow, absoluteColumn: false, absoluteRow: true);
            return $"{cellFrom}:{cellTo}";
        }

        public string GetRanksRange()
        {
            var cellFrom = Cell.At(1, EntriesFirstRow, absoluteColumn: true, absoluteRow: true);
            var cellTo = Cell.At(1, EntriesLastRow, absoluteColumn: true, absoluteRow: true);
            return $"{cellFrom}:{cellTo}";
        }

        public string GetAwardRange(JamAwardCriterion criterion)
        {
            var row = AwardRows[criterion];
            var cellFrom = Cell.At(VotesFirstColumn, row, absoluteColumn: true, absoluteRow: true);
            var cellTo = Cell.At(VotesLastColumn, row, absoluteColumn: true, absoluteRow: true);
            return $"{cellFrom}:{cellTo}";
        }

        public string GetUnjudgedRange()
        {
            var cellFrom = Cell.At(VotesFirstColumn, UnjudgedFirstRow, absoluteColumn: true, absoluteRow: true);
            var cellTo = Cell.At(VotesLastColumn, UnjudgedLastRow, absoluteColumn: true, absoluteRow: true);
            return $"{cellFrom}:{cellTo}";
        }
    }
}
