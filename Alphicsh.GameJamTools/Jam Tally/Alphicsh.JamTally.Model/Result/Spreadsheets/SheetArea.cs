namespace Alphicsh.JamTally.Model.Result.Spreadsheets
{
    internal abstract class SheetArea<TSheet>
        where TSheet : Sheet
    {
        public TSheet Sheet { get; }
        public int Left { get; }
        public int Top { get; }

        private bool HasRows { get; }
        public SheetCursor Cursor { get; }

        protected SheetArea(TSheet sheet, int left, int top, bool hasRows)        {
            Sheet = sheet;
            Left = left;
            Top = top;

            HasRows = hasRows;
            Cursor = new SheetCursor(Sheet, Left, Top);
        }

        protected SheetEntry CreateEntry()
        {
            var entry = new SheetEntry(Sheet, Cursor.Column, Cursor.Row, isRow: HasRows);

            if (HasRows)
                Cursor.NextRow();
            else
                Cursor.NextColumn();

            return entry;
        }
    }
}
