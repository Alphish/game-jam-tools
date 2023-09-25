using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets.Alignment
{
    internal class AlignmentSummaryArea : SheetArea<AlignmentSheet>
    {
        public AlignmentSummaryArea(AlignmentSheet sheet)
            : base(sheet, sheet.SummaryAlignmentNameColumn, sheet.SummaryHeaderRow, hasRows: true)
        {
        }

        public void Populate()
        {
            AddHeaderRow();
            foreach (var alignment in Sheet.AlignmentTally.AlignmentRanking)
            {
                AddSummaryRow(alignment);
            }
        }

        public void AddHeaderRow()
        {
            var row = CreateEntry();
            row.Add("GANG");
            row.Add("BASE SCORE");
            row.Add("MULTIPLIER");
            row.Add("THEME BONUS");
            row.Add("TOTAL");
        }

        public void AddSummaryRow(JamAlignmentOption alignment)
        {
            var row = CreateEntry();
            var referenceValue = $"\"{alignment.ShortTitle}\"";

            row.Add(alignment.Title);

            var baseScoreFrom = Cell.At(Sheet.VotesAlignmentColumn, Sheet.VotesAlignmentFirstRow, true, true);
            var baseScoreTo = Cell.At(Sheet.VotesAlignmentTotalColumn, Sheet.VotesAlignmentLastRow, true, true);
            var baseScoreRange = $"{baseScoreFrom}:{baseScoreTo}";
            var scoreLookupExpression = Formula.VlookupExpression(referenceValue, baseScoreRange, "2");
            row.Add("=" + scoreLookupExpression);

            var reviewAlignmentRange = Cell.ColumnRange(
                Sheet.ReviewsAlignmentNameColumn, Sheet.ReviewsAlignmentFirstRow, Sheet.ReviewsAlignmentLastRow,
                absoluteColumn: true, absoluteRow: true
                );
            var reviewMultiplierRange = Cell.ColumnRange(
                Sheet.ReviewsAlignmentMultiplierColumn, Sheet.ReviewsAlignmentFirstRow, Sheet.ReviewsAlignmentLastRow,
                absoluteColumn: true, absoluteRow: true
                );
            var multiplierLookupRange = $"{{{reviewAlignmentRange},{reviewMultiplierRange}}}";
            var multiplierLookupExpression = Formula.VlookupExpression(referenceValue, multiplierLookupRange, "2");
            row.Add("=" + multiplierLookupExpression);

            var themeBonus = Sheet.AlignmentTally.ThemeBonuses[alignment];
            row.Add(themeBonus > 0 ? themeBonus.ToString() : string.Empty);

            var baseScoreCell = Cell.At(row.Left + 1, row.Top, absoluteColumn: false, absoluteRow: false);
            var multiplierCell = Cell.At(row.Left + 2, row.Top, absoluteColumn: false, absoluteRow: false);
            var themeBonusCell = Cell.At(row.Left + 3, row.Top, absoluteColumn: false, absoluteRow: false);
            row.Add($"={baseScoreCell}*{multiplierCell}+{themeBonusCell}");
        }
    }
}
