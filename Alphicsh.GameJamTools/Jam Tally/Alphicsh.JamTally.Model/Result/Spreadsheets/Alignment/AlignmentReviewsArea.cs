using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets.Alignment
{
    internal class AlignmentReviewsArea : SheetArea<AlignmentSheet>
    {
        private HashSet<JamEntry> VotedEntries { get; }
        private HashSet<JamEntry> DuplicateEntries { get; }

        public AlignmentReviewsArea(AlignmentSheet sheet)
            : base(sheet, sheet.ReviewsAlignmentNameColumn, sheet.ReviewsHeaderRow, hasRows: true)
        {
            var eligibleVotes = Sheet.Workbook.TallyResult.Votes
                .Where(vote => vote.Ranking.Count >= 20 && vote.ReviewsCount >= 20)
                .ToList();
            VotedEntries = eligibleVotes.SelectMany(vote => vote.Authored).ToHashSet();

            var duplicates = JamTallyModel.Current.VotesCollection!.AlignmentBattleData?.Duplicates ?? new List<JamEntry>();
            DuplicateEntries = duplicates.ToHashSet();
        }

        public void Populate()
        {
            AddHeaderRow();
            foreach (var alignment in Sheet.Jam.Alignments!.GetAvailableOptions())
            {
                AddAlignmentRow(alignment);
            }

            Cursor.MoveTo(Sheet.ReviewsEntriesFirstColumn, Sheet.ReviewsEntriesFirstRow);
            foreach (var entry in Sheet.Jam.Entries)
            {
                AddEntryRow(entry);
            }
        }

        private void AddHeaderRow()
        {
            var row = CreateEntry();
            row.Add("GANG");
            row.Add("ELIGIBLE");
            row.Add("VOTELESS");
            row.Add("MULTIPLIER");
        }

        private void AddAlignmentRow(JamAlignmentOption alignment)
        {
            var row = CreateEntry();
            row.Add(alignment.ShortTitle);

            var referenceCell = Cell.At(row.Left, row.Top, absoluteColumn: false, absoluteRow: false);
            
            var votersRange = Cell.RowRange(Sheet.VotesHeaderRow, Sheet.VotesFirstColumn, Sheet.VotesLastColumn, true, true);
            var eligibleVotesExpression = Formula.CountIfEqualExpression(votersRange, referenceCell);
            row.Add("=" + eligibleVotesExpression);

            var alignmentsRange = Cell.ColumnRange(row.Left + 2, Sheet.ReviewsEntriesFirstRow, Sheet.ReviewsEntriesLastRow, true, true);
            var alignmentsCondition = $"\"=\"&" + referenceCell;
            var statusRange = Cell.ColumnRange(row.Left + 3, Sheet.ReviewsEntriesFirstRow, Sheet.ReviewsEntriesLastRow, true, true);
            var statusCondition = $"\"=Voteless\"";
            var votelessEntriesFormula = $"=COUNTIFS({alignmentsRange},{alignmentsCondition},{statusRange},{statusCondition})";
            row.Add(votelessEntriesFormula);

            var eligibleVotesCell = Cell.At(row.Left + 1, row.Top, absoluteColumn: false, absoluteRow: false);
            var votelessEntriesCell = Cell.At(row.Left + 2, row.Top, absoluteColumn: false, absoluteRow: false);
            row.Add($"=1+{eligibleVotesCell}/({eligibleVotesCell}+{votelessEntriesCell})");
        }

        private void AddEntryRow(JamEntry entry)
        {
            var row = CreateEntry();
            row.Add(entry.FullTitle);
            row.Add(entry.Team);
            row.Add(entry.Alignment?.ShortTitle ?? Sheet.Jam.Alignments!.NeitherTitle);

            if (VotedEntries.Contains(entry))
                row.Add("Eligible");
            else if (DuplicateEntries.Contains(entry))
                row.Add("Duplicate");
            else
                row.Add("Voteless");
        }
    }
}
