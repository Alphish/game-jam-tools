using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets.Alignment
{
    internal class AlignmentBaseScoreArea : SheetArea<AlignmentSheet>
    {
        public AlignmentBaseScoreArea(AlignmentSheet sheet)
            : base(sheet, sheet.VotesAlignmentColumn, sheet.VotesAlignmentFirstRow, hasRows: true)
        {
        }

        public void Populate()
        {
            foreach (var alignment in Sheet.Jam!.Alignments!.GetAvailableOptions())
            {
                AddTotalScoreRow(alignment);
            }
        }

        private void AddTotalScoreRow(JamAlignmentOption alignment)
        {
            var row = CreateEntry();
            row.Add(alignment.ShortTitle);

            var referenceCell = Cell.At(row.Left, row.Top, absoluteColumn: true, absoluteRow: false);
            var subtotalsFrom = Cell.At(Sheet.VotesFirstColumn, row.Top, absoluteColumn: true, absoluteRow: false);
            var subtotalsTo = Cell.At(Sheet.VotesLastColumn, row.Top, absoluteColumn: true, absoluteRow: false);
            var subtotalsRange = $"{subtotalsFrom}:{subtotalsTo}";

            var moduloExpression = $"MOD((COLUMN({subtotalsRange})-COLUMN({subtotalsFrom})),{Sheet.VoteWidth})";
            var sumifExpression = $"SUMIF(ARRAYFORMULA({moduloExpression}),{Sheet.VoteWidth - 1},{subtotalsRange})";
            var countifExpression = $"COUNTIF({subtotalsRange},\"=\"&{referenceCell})";
            var totalFormula = $"={sumifExpression}/{countifExpression}";
            row.Add(totalFormula);
        }
    }
}
