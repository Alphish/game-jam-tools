using ClosedXML.Excel;

namespace Alphicsh.JamTally.Spreadsheets
{
    public class SheetBuilder
    {
        public IXLWorksheet Worksheet { get; }

        protected int CurrentRow { get; set; } = 1;
        protected int CurrentColumn { get; set; } = 1;

        protected SheetBuilder(IXLWorksheet worksheet)
        {
            Worksheet = worksheet;
        }

        protected IXLRange Range(SheetSpan rows, SheetSpan columns)
            => Range(rows.From, columns.From, rows.To, columns.To);

        protected IXLRange Range(int rowFrom, int columnFrom, int rowTo, int columnTo)
            => Worksheet.Range(rowFrom, columnFrom, rowTo, columnTo);

        protected IXLRange Enter(params object[] contents)
        {
            var range = Range(CurrentRow, CurrentColumn, CurrentRow, contents.Length > 0 ? CurrentColumn + contents.Length - 1 : CurrentColumn);

            for (var i = 0; i < contents.Length; i++)
            {
                var content = contents[i];
                var cell = Worksheet.Cell(CurrentRow, CurrentColumn + i);

                if (content is string stringContent)
                {
                    if (stringContent.StartsWith("="))
                        cell.FormulaA1 = stringContent;
                    else
                        cell.Value = stringContent;
                }
                else if (content is int intContent)
                {
                    cell.Value = intContent;
                }
            }

            CurrentRow += 1;

            return range;
        }

        protected void DrawBox(SheetSpan rows, SheetSpan columns, XLColor color)
            => DrawBox(rows.From, columns.From, rows.To, columns.To, color);

        protected void DrawBox(int rowFrom, int columnFrom, int rowTo, int columnTo, XLColor color)
        {
            var range = Range(rowFrom, columnFrom, rowTo, columnTo);
            range.BoxWith(color, TallyStyles.BorderColor);
        }
    }
}
