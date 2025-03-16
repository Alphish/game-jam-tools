using ClosedXML.Excel;

namespace Alphicsh.JamTally.Spreadsheets
{
    public static class XLExtensions
    {
        public static string GetAddress(this IXLCell cell, bool fixColumn, bool fixRow)
        {
            var column = (fixColumn ? "$" : "") + cell.Address.ColumnLetter;
            var row = (fixRow ? "$" : "") + cell.Address.RowNumber;
            return $"{column}{row}";
        }

        public static string GetRelativeAddress(this IXLCell cell)
            => cell.GetAddress(fixColumn: false, fixRow: false);

        public static string GetFixedColumnAddress(this IXLCell cell)
            => cell.GetAddress(fixColumn: true, fixRow: false);

        public static string GetFixedRowAddress(this IXLCell cell)
            => cell.GetAddress(fixColumn: false, fixRow: true);

        public static string GetAbsoluteAddress(this IXLCell cell)
            => cell.GetAddress(fixColumn: true, fixRow: true);

        public static string GetAddress(this IXLRange range, bool fixColumns, bool fixRows)
            => range.FirstCell().GetAddress(fixColumns, fixRows) + ":" + range.LastCell().GetAddress(fixColumns, fixRows);

        public static string GetRelativeAddress(this IXLRange range)
            => range.GetAddress(fixColumns: false, fixRows: false);

        public static string GetFixedColumnAddress(this IXLRange range)
            => range.GetAddress(fixColumns: true, fixRows: false);

        public static string GetFixedRowAddress(this IXLRange range)
            => range.GetAddress(fixColumns: false, fixRows: true);

        public static string GetAbsoluteAddress(this IXLRange range)
            => range.GetAddress(fixColumns: true, fixRows: true);

        public static void FillWith(this IXLCell cell, XLColor color)
        {
            cell.Style.Fill.BackgroundColor = color;
        }

        public static void FillWith(this IXLRange range, XLColor color)
        {
            range.Style.Fill.BackgroundColor = color;
        }

        public static void BoxWith(this IXLRange range, XLColor fillColor, XLColor borderColor)
        {
            range.Style.Fill.BackgroundColor = fillColor;
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            range.Style.Border.OutsideBorderColor = borderColor;
        }
    }
}
