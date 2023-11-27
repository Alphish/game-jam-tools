using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets.Ranking
{
    internal class RankingVoteArea : SheetArea<RankingSheet>
    {
        public JamVote Vote { get; }

        public RankingVoteArea(RankingSheet sheet, JamVote vote)
            : base(sheet, sheet.VoteColumns[vote], sheet.HeaderRow, hasRows: true)
        {
            Vote = vote;
        }

        public void Populate()
        {
            AddHeaderRow();
            foreach (var entry in Sheet.TallyResult.FinalRanking)
            {
                AddEntryRow(entry.Entry);
            }
            AddFooterRow();
        }

        public void AddHeaderRow()
        {
            var row = CreateEntry();
            row.Add(Vote.Voter!);

            if (Sheet.Workbook.HasAlignments)
                row.Add(Vote.Alignment?.ShortTitle ?? Sheet.Jam.Alignments!.NeitherTitle);
        }

        public void AddEntryRow(JamEntry entry)
        {
            var row = CreateEntry();
            row.Add(GetRankFormula(row.Top));
            row.Add(GetScoreFormula(row));
        }

        private string GetRankFormula(int row)
        {
            var titleCell = Sheet.TitleCellFor(row);
            var votesSheet = Sheet.Workbook.VotesSheet;
            var referenceRange = $"Votes!{votesSheet.GetVoteTitleRange(Vote)}";
            var ranksRange = $"Votes!{votesSheet.GetRanksRange()}";
            var lookupExpression = $"VLOOKUP({titleCell},{{{referenceRange},{ranksRange}}},2,FALSE)";
            var rankFormula = $"=IFNA({lookupExpression},)";
            return rankFormula;
        }

        private string GetScoreFormula(SheetEntry row)
        {
            var rankCell = Cell.At(row.Left, row.Top, absoluteColumn: false, absoluteRow: false);
            var scoreExpression = $"1/({rankCell}+1)";
            return $"=IF({rankCell}>0,{scoreExpression},)";
        }

        public void AddFooterRow()
        {
            var row = CreateEntry();
            var ranksRange = Sheet.GetVoteRanksRange(row.Left);
            var formula = $"=SQRT(1/4+2*SUM({ranksRange}))-1/2";
            row.Add(formula);
        }
    }
}
