using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets.Ranking
{
    internal class RankingAwardsArea : SheetArea<RankingSheet>
    {
        public RankingAwardsArea(RankingSheet sheet)
            : base(sheet, sheet.AwardsFirstColumn, sheet.HeaderRow, hasRows: true)
        {
        }

        public void Populate()
        {
            AddHeaderRow();
            foreach (var entry in Sheet.TallyResult.FinalRanking)
            {
                AddEntryRow(entry.Entry);
            }
        }

        private void AddHeaderRow()
        {
            var row = CreateEntry();
            foreach (var award in Sheet.TallyResult.Awards)
            {
                row.Add(award.Id);
            }
        }

        private void AddEntryRow(JamEntry entry)
        {
            var row = CreateEntry();
            var votesSheet = Sheet.Workbook.VotesSheet;
            var titleCell = Sheet.TitleCellFor(row.Top);

            foreach (var award in Sheet.TallyResult.Awards)
            {
                var entryRange = $"Votes!{votesSheet.GetAwardRange(award)}";
                var condition = $"\"=\"&{titleCell}";
                var formula = Formula.CountIfOrEmptyFormula(entryRange, condition);
                row.Add(formula);
            }
        }
    }
}
