using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Result.Alignments;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets.Alignment
{
    internal class AlignmentVoteArea : SheetArea<AlignmentSheet>
    {
        public JamAlignmentVote AlignmentVote { get; }
        public JamVote Vote => AlignmentVote.OriginalVote;

        public AlignmentVoteArea(AlignmentSheet sheet, JamAlignmentVote vote)
            : base(sheet, sheet.VotesColumns[vote.OriginalVote], sheet.VotesHeaderRow, hasRows: true)
        {
            AlignmentVote = vote;
        }

        public void Populate()
        {
            AddHeaderRow();

            Cursor.MoveTo(Left, Sheet.VotesAlignmentFirstRow);
            AddTotalCountRow();

            Cursor.MoveTo(Left + 2, Sheet.VotesAlignmentFirstRow);
            foreach (var alignment in Sheet.Jam!.Alignments!.GetAvailableOptions())
            {
                AddAlignmentRow(alignment);
            }

            var entryScoresByEntry = AlignmentVote.EntryScores.ToDictionary(entryScore => entryScore.Entry);

            Cursor.MoveTo(Left, Sheet.RankingFirstRow);
            foreach (var entry in Vote.Ranking)
            {
                var entryScore = entryScoresByEntry.ContainsKey(entry) ? entryScoresByEntry[entry] : null;
                AddVoteEntryRow(entry, entryScore);
            }

            Cursor.MoveTo(Left, Sheet.UnrankedFirstRow);
            foreach (var entry in Vote.Missing)
            {
                var entryScore = entryScoresByEntry.ContainsKey(entry) ? entryScoresByEntry[entry] : null;
                AddVoteEntryRow(entry, entryScore);
            }
        }

        private void AddHeaderRow()
        {
            var row = CreateEntry();
            
            row.Add(Vote.Voter!);
            row.Add(string.Empty);
            row.Add(string.Empty);

            if (Vote.Alignment == null)
                row.Add(Sheet.Jam.Alignments!.NeitherTitle);
            else if (Vote.Ranking.Count >= 20 && Vote.ReviewsCount >= 20)
                row.Add(Vote.Alignment!.ShortTitle);
            else
                row.Add(Vote.Alignment!.ShortTitle + " (N/E)");
        }

        private void AddTotalCountRow()
        {
            var row = CreateEntry();
            row.Add("Total");

            var cellFrom = Cell.At(Left + 3, Sheet.VotesAlignmentFirstRow, absoluteColumn: false, absoluteRow: true);
            var cellTo = Cell.At(Left + 3, Sheet.VotesAlignmentLastRow, absoluteColumn: false, absoluteRow: true);
            var sumFormula = $"=SUM({cellFrom}:{cellTo})";
            row.Add(sumFormula);
        }

        private void AddAlignmentRow(JamAlignmentOption alignment)
        {
            var row = CreateEntry();
            if (Vote.Alignment == alignment)
                return;

            row.Add(alignment.ShortTitle);

            var alignmentFrom = Cell.At(Left + 2, Sheet.RankingFirstRow, absoluteColumn: false, absoluteRow: true);
            var alignmentTo = Cell.At(Left + 2, Sheet.UnrankedLastRow, absoluteColumn: false, absoluteRow: true);
            var alignmentRange = $"{alignmentFrom}:{alignmentTo}";
            var voteAlignmentCell = Cell.At(row.Left, row.Top, absoluteColumn: false, absoluteRow: false);
            var condition = $"\"=\"&{voteAlignmentCell}";
            var alignedCountFormula = Formula.CountIfOrEmptyFormula(alignmentRange, condition);
            row.Add(alignedCountFormula);

            var totalCell = Cell.At(Left + 1, Sheet.VotesAlignmentFirstRow, absoluteColumn: false, absoluteRow: true);
            var countCell = Cell.At(row.Left + 1, row.Top, absoluteColumn: false, absoluteRow: false);
            var competingCount = $"={totalCell}-{countCell}";
            row.Add(competingCount);

            var totalFrom = Cell.At(Left + 5, Sheet.RankingFirstRow, absoluteColumn: false, absoluteRow: true);
            var totalTo = Cell.At(Left + 5, Sheet.UnrankedLastRow, absoluteColumn: false, absoluteRow: true);
            var totalRange = $"{totalFrom}:{totalTo}";
            var totalSum = Formula.SumIfExpression(alignmentRange, condition, totalRange);
            var averageSum = $"={totalSum}/{countCell}";
            row.Add(averageSum);
        }

        private void AddVoteEntryRow(JamEntry entry, JamAlignmentEntryScore? entryScore)
        {
            var row = CreateEntry();
            row.Add(entry.FullTitle);
            row.Add(entry.Team);
            row.Add(entry.Alignment?.ShortTitle ?? Sheet.Jam.Alignments!.NeitherTitle);
            if (entryScore == null)
                return;

            // competing entries below
            if (entryScore.IsRanked)
                row.Add(entryScore.CompetingEntriesBelow.ToString());
            else
                row.Add($"0-{entryScore.CompetingEntriesBelow}");

            // competing entries total
            var alignmentCell = Cell.At(row.Left + 2, row.Top, absoluteColumn: false, absoluteRow: false);
            var alignmentRange = GetVoteAlignmentRange();
            var competingCountRange = GetCompetingCountRange();
            var lookupRange = $"{{{alignmentRange},{competingCountRange}}}";
            var lookupExpression = Formula.VlookupExpression(alignmentCell, lookupRange, "2");
            row.Add($"={lookupExpression}");

            // score
            var competingBelowCell = entryScore.IsRanked
                ? Cell.At(row.Left + 3, row.Top, absoluteColumn: false, absoluteRow: false)
                : $"({entryScore.CompetingEntriesBelow}/2)";
            var competingTotalCell = Cell.At(row.Left + 4, row.Top, absoluteColumn: false, absoluteRow: false);
            row.Add($"={competingBelowCell}/{competingTotalCell}");
        }

        private string GetVoteAlignmentRange()
        {
            var cellFrom = Cell.At(Left + 2, Sheet.VotesAlignmentFirstRow, absoluteColumn: false, absoluteRow: true);
            var cellTo = Cell.At(Left + 2, Sheet.VotesAlignmentLastRow, absoluteColumn: false, absoluteRow: true);
            return $"{cellFrom}:{cellTo}";
        }

        private string GetCompetingCountRange()
        {
            var cellFrom = Cell.At(Left + 4, Sheet.VotesAlignmentFirstRow, absoluteColumn: false, absoluteRow: true);
            var cellTo = Cell.At(Left + 4, Sheet.VotesAlignmentLastRow, absoluteColumn: false, absoluteRow: true);
            return $"{cellFrom}:{cellTo}";
        }
    }
}
