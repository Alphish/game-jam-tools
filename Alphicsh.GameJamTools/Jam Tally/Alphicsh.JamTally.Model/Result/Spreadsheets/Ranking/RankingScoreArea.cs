using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets.Ranking
{
    internal class RankingScoreArea : SheetArea<RankingSheet>
    {
        private JamTallyResult TallyResult { get; }

        public RankingScoreArea(RankingSheet sheet)
            : base(sheet, sheet.ScoreFirstColumn, sheet.HeaderRow, hasRows: true)
        {
            TallyResult = Sheet.Workbook.TallyResult;
        }

        public void Populate()
        {
            AddHeaderRow();
            foreach (var entry in TallyResult.FinalRanking)
            {
                AddEntryRow(entry.Entry);
            }
            AddFooterRow();
        }

        private void AddHeaderRow()
        {
            var row = CreateEntry();
            row.Add("SCORE");
            row.Add("BASE");
            row.Add("UJ");
        }

        private void AddEntryRow(JamEntry entry)
        {
            var row = CreateEntry();
            
            row.Add(GetTotalCell(row.Top));
            row.Add(GetBaseScoreCell(row.Top));
            row.Add(GetUnjudgedCountCell(row.Top));
        }

        private string GetTotalCell(int row)
        {
            var baseScoreCell = Cell.At(Left + 1, row);

            var votesCountCell = Cell.At(Left + 1, Sheet.FooterRow, absoluteColumn: true, absoluteRow: true);
            var unjudgedCountCell = Cell.At(Left + 2, row);
            var scoreModifier = $"{votesCountCell}/({votesCountCell}-{unjudgedCountCell})";

            return $"={baseScoreCell}*{scoreModifier}";
        }

        private string GetBaseScoreCell(int row)
        {
            var scoreRange = Sheet.GetEntryVotesRange(row);
            var isEvenVote = Sheet.VotesFirstColumn % 2 == 0;
            var productWith = isEvenVote ? $"mod(column({scoreRange}),2)" : $"1-mod(column({scoreRange}),2)";
            var formula = $"=ARRAYFORMULA(SUMPRODUCT({productWith},{scoreRange}))";
            return formula;
        }

        private string GetUnjudgedCountCell(int row)
        {
            var titleCell = Sheet.TitleCellFor(row);
            var unjudgedRange = $"Votes!{Sheet.Workbook.VotesSheet.GetUnjudgedRange()}";
            var condition = $"\"=\"&{titleCell}";
            var formula = Formula.CountIfOrEmptyFormula(unjudgedRange, condition);
            return formula;
        }

        private void AddFooterRow()
        {
            var row = CreateEntry();
            row.Add(string.Empty);

            var voteFooterRange = Sheet.GetVoteFooterRange();
            row.Add($"=COUNTIF({voteFooterRange},\">0\")");

            var unjudgedCountsRange = Sheet.GetUnjudgedCountsRange();
            row.Add($"=SUM({unjudgedCountsRange})");
        }
    }
}
