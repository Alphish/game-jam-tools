namespace Alphicsh.JamTally.Model.Result.Spreadsheets
{
    internal class SheetEntry
    {
        public Sheet Sheet { get; }
        public int Left { get; }
        public int Top { get; }

        public bool IsRow { get; }
        public SheetCursor Cursor { get; }

        public SheetEntry(Sheet sheet, int left, int top, bool isRow)
        {
            Sheet = sheet;
            Left = left;
            Top = top;

            IsRow = isRow;
            Cursor = new SheetCursor(Sheet, Left, Top);
        }

        public void Add(string content)
        {
            Cursor.Set(content);

            if (IsRow)
                Cursor.NextColumn();
            else
                Cursor.NextRow();
        }
    }
}
