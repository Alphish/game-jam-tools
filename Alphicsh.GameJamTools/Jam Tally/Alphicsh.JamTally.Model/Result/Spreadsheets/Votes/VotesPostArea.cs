using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets.Votes
{
    internal class VotesPostArea : SheetArea<VotesSheet>
    {
        private JamVote Vote { get; }

        public VotesPostArea(VotesSheet sheet, JamVote vote)
            : base(sheet, sheet.VoteColumns[vote], sheet.HeaderRow, hasRows: true)
        {
            Vote = vote;
        }

        public void Populate()
        {
            var entriesRange = "Ranking!" + Sheet.Workbook.RankingSheet.GetEntriesRange();

            Cursor.MoveTo(Left, Top);
            AddHeaderRow();

            Cursor.MoveTo(Left, Sheet.EntriesFirstRow);
            foreach (var entry in Vote.Ranking)
            {
                AddEntryRow(entry, entriesRange);
            }

            Cursor.MoveTo(Left, Sheet.AwardsFirstRow);
            var awardsMap = Vote.Awards.ToDictionary(award => award.Criterion, award => award.Entry);
            foreach (var award in Sheet.TallyResult.Awards)
            {
                var entry = awardsMap.TryGetValue(award, out var foundEntry) ? foundEntry : null;
                AddEntryRow(entry, entriesRange);
            }

            Cursor.MoveTo(Left, Sheet.UnjudgedFirstRow);
            foreach (var entry in Vote.Authored.Union(Vote.Unjudged))
            {
                AddEntryRow(entry, entriesRange);
            }

            Cursor.MoveTo(Left, Sheet.ReactionsFirstRow);
            foreach (var reaction in Vote.AggregateReactions)
            {
                AddReactionRow(reaction);
            }

            Cursor.MoveTo(Left, Sheet.ReactionsTotalRow);
            AddReactionTotalRow();
        }

        private void AddHeaderRow()
        {
            var row = CreateEntry();
            row.Add(Vote.Voter!);

            if (Sheet.Workbook.HasAlignments)
                row.Add(Vote.Alignment?.ShortTitle ?? Sheet.Workbook.Jam.Alignments!.NeitherTitle);
        }

        private void AddEntryRow(JamEntry? entry, string entriesRange)
        {
            var row = CreateEntry();

            row.Add(entry?.FullTitle ?? string.Empty);

            var titleCell = Cell.At(row.Left, row.Top);
            var authorsReference = $"=IF({titleCell}=\"\",,VLOOKUP({titleCell},{entriesRange},2,FALSE))";
            row.Add(authorsReference);
        }

        private void AddReactionRow(JamVoteReaction reaction)
        {
            var row = CreateEntry();
            row.Add(reaction.Name);
            row.Add(reaction.Value.ToString());
        }

        private void AddReactionTotalRow()
        {
            var row = CreateEntry();

            row.Add("Total");

            var reactionsFrom = Cell.At(Left + 1, Sheet.ReactionsFirstRow);
            var reactionsTo = Cell.At(Left + 1, Sheet.ReactionsLastRow);
            var reactionsSum = $"=SUM({reactionsFrom}:{reactionsTo})";
            row.Add(reactionsSum);
        }
    }
}
