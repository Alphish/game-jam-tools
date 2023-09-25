using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets.Ranking
{
    internal class RankingEntriesArea : SheetArea<RankingSheet>
    {
        public RankingEntriesArea(RankingSheet sheet)
            : base(sheet, sheet.EntriesFirstColumn, sheet.HeaderRow, hasRows: true)
        {
        }

        // ----------
        // Populating
        // ----------

        public void Populate()
        {
            AddHeaderRow();
            foreach (var entry in Sheet.Jam.Entries)
            {
                AddEntryRow(entry);
            }
        }

        private void AddHeaderRow()
        {
            var row = CreateEntry();
            row.Add("GAME");
            row.Add("AUTHORS");
            if (Sheet.Workbook.HasAlignments)
            {
                row.Add("GANG");
            }
        }

        private void AddEntryRow(JamEntry entry)
        {
            var row = CreateEntry();
            row.Add(entry.FullTitle);
            row.Add(entry.Team);

            if (Sheet.Workbook.HasAlignments)
                row.Add(entry.Alignment?.ShortTitle ?? Sheet.Jam.Alignments!.NeitherTitle);
        }
    }
}
