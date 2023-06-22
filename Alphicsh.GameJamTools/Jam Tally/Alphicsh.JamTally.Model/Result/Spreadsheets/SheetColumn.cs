using System.Collections.Generic;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets
{
    internal class SheetColumn
    {
        public IList<string> Cells { get; } = new List<string>();

        public int Count => Cells.Count;

        public void Add(string cell)
        {
            Cells.Add(cell);
        }
    }
}
