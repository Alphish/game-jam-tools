namespace Alphicsh.JamTally.Model.Result.Spreadsheets
{
    internal static class Cell
    {
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
    }
}
