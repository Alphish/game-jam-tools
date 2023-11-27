using System;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets
{
    internal class Sheet
    {
        private List<List<string>> Cells { get; } = new List<List<string>>();

        public void ExpandTo(int width, int height)
        {
            while (Cells.Count < height)
            {
                Cells.Add(new List<string>());
            }

            foreach (var row in Cells)
            {
                while (row.Count < width)
                    row.Add(string.Empty);
            }
        }

        public void Set(int row, int column, string content)
        {
            if (Cells[row - 1][column - 1] != string.Empty)
                throw new InvalidOperationException($"The cell at ({row},{column}) already has a value '{content}'.");

            Cells[row - 1][column - 1] = content;
        }

        // --------------
        // Generating CSV
        // --------------

        public string GenerateCsv()
        {
            var sb = new StringBuilder();
            var rowsCount = Cells.Count;
            for (var i = 0; i < rowsCount; i++)
            {
                WriteRow(i, sb);
            }
            return sb.ToString();
        }

        private void WriteRow(int row, StringBuilder sb)
        {
            var isFirst = true;
            foreach (var cell in Cells[row])
            {
                if (!isFirst)
                    sb.Append("\t");

                sb.Append("\"");
                sb.Append(cell.Replace("\"", "\"\""));
                sb.Append("\"");

                isFirst = false;
            }
            sb.Append("\n");
        }

    }
}
