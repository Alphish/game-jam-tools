using System.Linq;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets
{
    internal class RankingSheetGenerator
    {
        public string Generate(JamTallyResult tallyResult)
        {
            var sheet = GenerateSheet(tallyResult);
            return sheet.GenerateCsv();
        }

        private Sheet GenerateSheet(JamTallyResult tallyResult)
        {
            var sheet = new Sheet();

            sheet.AddColumn(MakeTitlesColumn(tallyResult));
            sheet.AddColumn(MakeAuthorsColumn(tallyResult));

            sheet.AddColumn(MakeTotalScoreColumn(tallyResult));
            sheet.AddColumn(MakeBaseScoreColumn(tallyResult));
            sheet.AddColumn(MakeUnjudgedEntriesColumn(tallyResult));

            for (var i = 0; i < tallyResult.AwardsCount; i++)
            {
                sheet.AddColumn(MakeAwardColumn(tallyResult, i));
            }

            for (var i = 0; i < tallyResult.Votes.Count; i++)
            {
                var vote = tallyResult.Votes.ElementAt(i);
                sheet.AddColumn(MakeVoteMainColumn(tallyResult, vote, i));
                sheet.AddColumn(MakeVoteAuxiliaryColumn(tallyResult, i));
            }
            return sheet;
        }

        // ----------
        // Entry data
        // ----------

        private SheetColumn MakeTitlesColumn(JamTallyResult tallyResult)
        {
            var column = new SheetColumn();
            column.Add("GAME");

            foreach (var entryScore in tallyResult.FinalRanking)
            {
                column.Add(entryScore.Entry.Title);
            }

            column.Add("");
            return column;
        }

        private SheetColumn MakeAuthorsColumn(JamTallyResult tallyResult)
        {
            var column = new SheetColumn();
            column.Add("AUTHOR(S)\\VOTER(S)");

            foreach (var entryScore in tallyResult.FinalRanking)
            {
                column.Add(entryScore.Entry.Team);
            }

            column.Add("");
            return column;
        }

        // -----
        // Score
        // -----

        private SheetColumn MakeTotalScoreColumn(JamTallyResult tallyResult)
        {
            var column = new SheetColumn();
            column.Add("SCORE");

            for (var i = 0; i < tallyResult.EntriesCount; i++)
            {
                var baseScoreCell = Sheet.At(4, i + 2);
                var totalVotesCell = Sheet.At(4, 2 + tallyResult.EntriesCount, true, true);
                var unjudgedCell = Sheet.At(5, i + 2);
                column.Add($"={baseScoreCell}*{totalVotesCell}/({totalVotesCell}-{unjudgedCell})");
            }

            column.Add("");
            return column;
        }

        private SheetColumn MakeBaseScoreColumn(JamTallyResult tallyResult)
        {
            var column = new SheetColumn();
            column.Add("BASE");

            var startVotesColumn = 6 + tallyResult.AwardsCount;
            var endVotesColumn = startVotesColumn + 2 * tallyResult.Votes.Count - 1;
            for (var i = 0; i < tallyResult.EntriesCount; i++)
            {
                var startRangeCell = Sheet.At(startVotesColumn, i + 2);
                var endRangeCell = Sheet.At(endVotesColumn, i + 2);
                var cellRange = startRangeCell + ":" + endRangeCell;
                var func = tallyResult.AwardsCount % 2 == 0 ? "mod" : "1-mod";
                column.Add($"=ARRAYFORMULA(SUMPRODUCT({func}(column({cellRange}),2), {cellRange}))");
            }

            var footerRow = 2 + tallyResult.EntriesCount;
            var footerRange = Sheet.At(startVotesColumn, footerRow) + ":" + Sheet.At(endVotesColumn, footerRow);
            column.Add($"=COUNTIF({footerRange},\">0\")");

            return column;
        }

        private SheetColumn MakeUnjudgedEntriesColumn(JamTallyResult tallyResult)
        {
            var column = new SheetColumn();
            column.Add("UJ");

            var startUnjudgedColumn = 2;
            var endUnjudgedColumn = startUnjudgedColumn + 2 * tallyResult.Votes.Count - 1;
            var startUnjudgedRow = 2 + tallyResult.EntriesCount + tallyResult.AwardsCount;
            var endUnjudgedRow = startUnjudgedRow + tallyResult.UnjudgedMaxCount - 1;
            var startRangeCell = Sheet.At(startUnjudgedColumn, startUnjudgedRow, true, true);
            var endRangeCell = Sheet.At(endUnjudgedColumn, endUnjudgedRow, true, true);
            var cellRange = "Votes!" + startRangeCell + ":" + endRangeCell;

            for (var i = 0; i < tallyResult.EntriesCount; i++)
            {
                column.Add($"=IF(COUNTIF({cellRange}, \"=\"&$A{i + 2})>0,COUNTIF({cellRange}, \"=\"&$A{i + 2}),)");
            }

            var footerRange = Sheet.At(5, 2) + ":" + Sheet.At(5, 1 + tallyResult.EntriesCount);
            column.Add($"=SUM({footerRange})");

            return column;
        }

        // ------
        // Awards
        // ------

        private SheetColumn MakeAwardColumn(JamTallyResult tallyResult, int awardIndex)
        {
            var award = tallyResult.Awards.ElementAt(awardIndex);

            var column = new SheetColumn();
            column.Add(award.Id.ToUpperInvariant());

            var startAwardColumn = 2;
            var endAwardColumn = startAwardColumn + 2 * tallyResult.Votes.Count - 1;
            var startAwardRow = 2 + tallyResult.EntriesCount + awardIndex;
            var endAwardRow = startAwardRow;
            var startRangeCell = Sheet.At(startAwardColumn, startAwardRow, true, true);
            var endRangeCell = Sheet.At(endAwardColumn, endAwardRow, true, true);
            var cellRange = "Votes!" + startRangeCell + ":" + endRangeCell;

            for (var i = 0; i < tallyResult.EntriesCount; i++)
            {
                column.Add($"=IF(COUNTIF({cellRange}, \"=\"&$A{i + 2})>0,COUNTIF({cellRange}, \"=\"&$A{i + 2}),)");
            }

            column.Add("");
            return column;
        }

        // -----
        // Votes
        // -----

        private SheetColumn MakeVoteMainColumn(JamTallyResult tallyResult, JamVote vote, int voteIndex)
        {
            var column = new SheetColumn();
            column.Add(vote.Voter ?? "<voter>");
            var columnIndex = 6 + tallyResult.AwardsCount + 2 * voteIndex;

            var startVotesColumn = 2 + 2 * voteIndex;
            var endVotesColumn = startVotesColumn;
            var startVotesRow = 2;
            var endVotesRow = 1 + tallyResult.EntriesCount;
            var startRangeCell = Sheet.At(startVotesColumn, startVotesRow, false, true);
            var endRangeCell = Sheet.At(endVotesColumn, endVotesRow, false, true);
            var voteRange = "Votes!" + startRangeCell + ":" + endRangeCell;
            var rankRange = "Votes!" + Sheet.At(1, startVotesRow) + ":" + Sheet.At(1, endVotesRow);

            for (var i = 0; i < tallyResult.EntriesCount; i++)
            {
                column.Add($"=IFNA(VLOOKUP($A{i+2},{{{voteRange},{rankRange}}},2,FALSE),)");
            }

            var footerRange = Sheet.At(columnIndex, 2, false, true) + ":" + Sheet.At(columnIndex, 1 + tallyResult.EntriesCount);
            column.Add($"=SQRT(1/4 + 2*SUM({footerRange})) - 1/2");

            return column;
        }

        private SheetColumn MakeVoteAuxiliaryColumn(JamTallyResult tallyResult, int voteIndex)
        {
            var column = new SheetColumn();
            column.Add("");
            var columnIndex = 6 + tallyResult.AwardsCount + 2 * voteIndex + 1;

            for (var i = 0; i < tallyResult.EntriesCount; i++)
            {
                var rankCell = Sheet.At(columnIndex - 1, i + 2);
                column.Add($"=if({rankCell}>0,1/({rankCell}+1),\"\")");
            }

            column.Add("");
            return column;
        }
    }
}
