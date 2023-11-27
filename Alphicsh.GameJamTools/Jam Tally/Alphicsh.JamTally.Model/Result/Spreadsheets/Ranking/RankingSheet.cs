using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets.Ranking
{
    internal class RankingSheet : Sheet
    {
        public TallyWorkbook Workbook { get; }
        public JamOverview Jam { get; }
        public JamTallyResult TallyResult { get; }

        public int HeaderRow { get; }
        public int EntriesFirstRow { get; }
        public int EntriesLastRow => EntriesFirstRow + TallyResult.EntriesCount - 1;
        public int FooterRow { get; }

        public int EntriesFirstColumn { get; }
        public int EntriesWidth => Workbook.HasAlignments ? 3 : 2;
        public int EntriesLastColumn => EntriesFirstColumn + EntriesWidth - 1;

        public int ScoreFirstColumn { get; }
        public int ScoreLastColumn => ScoreFirstColumn + 2;

        public int AwardsFirstColumn { get; }
        public int AwardsLastColumn => AwardsFirstColumn + TallyResult.AwardsCount - 1;

        public int VotesFirstColumn { get; }
        public int VotesLastColumn => VotesFirstColumn + TallyResult.Votes.Count * 2 - 1;
        public IReadOnlyDictionary<JamVote, int> VoteColumns { get; }

        public int TotalWidth => VotesLastColumn;
        public int TotalHeight => FooterRow;

        public RankingSheet(TallyWorkbook workbook)
        {
            Workbook = workbook;
            Jam = Workbook.Jam;
            TallyResult = Workbook.TallyResult;

            HeaderRow = 1;
            EntriesFirstRow = HeaderRow + 1;
            FooterRow = EntriesLastRow + 1;

            EntriesFirstColumn = 1;
            ScoreFirstColumn = EntriesLastColumn + 1;
            AwardsFirstColumn = ScoreLastColumn + 1;
            VotesFirstColumn = AwardsLastColumn + 1;
            VoteColumns = MakeVoteColumns();

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

        // ----------
        // Populating
        // ----------

        public void Populate()
        {
            var entriesArea = new RankingEntriesArea(this);
            entriesArea.Populate();

            var scoreArea = new RankingScoreArea(this);
            scoreArea.Populate();

            var awardsArea = new RankingAwardsArea(this);
            awardsArea.Populate();

            foreach (var vote in TallyResult.Votes)
            {
                var voteArea = new RankingVoteArea(this, vote);
                voteArea.Populate();
            }
        }

        // ----------------
        // Helper functions
        // ----------------

        public string TitleCellFor(int row)
        {
            return Cell.At(EntriesFirstColumn, row, absoluteColumn: true, absoluteRow: false);
        }

        public string GetVoteRanksRange(int column)
        {
            var cellFrom = Cell.At(column, EntriesFirstRow, absoluteColumn: false, absoluteRow: true);
            var cellTo = Cell.At(column, EntriesLastRow, absoluteColumn: false, absoluteRow: true);
            return $"{cellFrom}:{cellTo}";
        }

        public string GetEntriesRange()
        {
            var topLeft = Cell.At(EntriesFirstColumn, EntriesFirstRow, absoluteColumn: true, absoluteRow: true);
            var bottomRight = Cell.At(EntriesFirstColumn + 1, EntriesLastRow, absoluteColumn: true, absoluteRow: true);
            return $"{topLeft}:{bottomRight}";
        }

        public string GetEntryVotesRange(int row)
        {
            var cellFrom = Cell.At(VotesFirstColumn, row, absoluteColumn: true, absoluteRow: false);
            var cellTo = Cell.At(VotesLastColumn, row, absoluteColumn: true, absoluteRow: false);
            return $"{cellFrom}:{cellTo}";
        }

        public string GetVoteFooterRange()
        {
            var cellFrom = Cell.At(VotesFirstColumn, FooterRow, absoluteColumn: true, absoluteRow: false);
            var cellTo = Cell.At(VotesLastColumn, FooterRow, absoluteColumn: true, absoluteRow: false);
            return $"{cellFrom}:{cellTo}";
        }

        public string GetUnjudgedCountsRange()
        {
            var cellFrom = Cell.At(ScoreLastColumn, EntriesFirstRow, absoluteColumn: true, absoluteRow: true);
            var cellTo = Cell.At(ScoreLastColumn, EntriesLastRow, absoluteColumn: true, absoluteRow: true);
            return $"{cellFrom}:{cellTo}";
        }
    }
}
