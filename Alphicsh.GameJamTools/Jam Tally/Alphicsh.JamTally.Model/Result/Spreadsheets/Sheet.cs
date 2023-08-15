using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets
{
    internal class Sheet
    {
        private IList<SheetColumn> Columns { get; } = new List<SheetColumn>();

        public void AddColumn(SheetColumn column)
        {
            Columns.Add(column);
        }

        // ----------------
        // Formatting cells
        // ----------------

        private static char[] Alphabet { get; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public static string At(int column, int row, bool absoluteColumn = false, bool absoluteRow = false)
        {
            var remainder = column - 1;
            var letterIdx = remainder % 26;
            var columnName = Alphabet[letterIdx].ToString();
            remainder -= letterIdx;

            while (remainder > 0)
            {
                remainder = (remainder / 26) - 1;
                letterIdx = remainder % 26;
                columnName = Alphabet[letterIdx] + columnName;
                remainder -= letterIdx;
            }

            if (absoluteColumn)
                columnName = "$" + columnName;

            var rowName = absoluteRow ? "$" + row.ToString() : row.ToString();
            return columnName + rowName;
        }

        // --------------
        // Generating CSV
        // --------------

        public string GenerateCsv()
        {
            var sb = new StringBuilder();
            var rowsCount = Columns.First().Count;
            for (var i = 0; i < rowsCount; i++)
            {
                PopulateRow(i, sb);
            }
            return sb.ToString();
        }

        private void PopulateRow(int row, StringBuilder sb)
        {
            var isFirst = true;
            foreach (var column in Columns)
            {
                if (!isFirst)
                    sb.Append("\t");

                sb.Append("\"");
                sb.Append(column.Cells[row].Replace("\"", "\"\""));
                sb.Append("\"");

                isFirst = false;
            }
            sb.Append("\n");
        }

    }
}
