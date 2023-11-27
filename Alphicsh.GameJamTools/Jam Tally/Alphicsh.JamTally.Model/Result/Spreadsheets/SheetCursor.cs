namespace Alphicsh.JamTally.Model.Result.Spreadsheets
{
    internal class SheetCursor
    {
        public Sheet Sheet { get; }
        public int Column { get; private set; }
        public int Row { get; private set; }

        public SheetCursor(Sheet sheet, int column, int row)
        {
            Sheet = sheet;
            Column = column;
            Row = row;
        }

        public void MoveTo(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public void NextRow()
        {
            Row++;
        }

        public void NextColumn()
        {
            Column++;
        }

        public void Set(string content)
        {
            Sheet.Set(Row, Column, content);
        }
    }
}
