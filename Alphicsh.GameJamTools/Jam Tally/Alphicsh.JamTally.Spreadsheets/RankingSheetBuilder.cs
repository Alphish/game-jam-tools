using System.Linq;
using Alphicsh.JamTally.Model.Result;
using ClosedXML.Excel;

namespace Alphicsh.JamTally.Spreadsheets
{
    public class RankingSheetBuilder : SheetBuilder
    {
        private JamTallyNewResult TallyResult { get; }

        public SheetSpan HeaderRows { get; }
        public SheetSpan EntriesRows { get; }
        public SheetSpan FooterRows { get; }

        public SheetSpan EntriesColumns { get; }
        public SheetSpan ScoreColumns { get; }
        public SheetSpan AwardColumns { get; }
        public SheetSpan VoteColumns { get; }
        public int ColumnsPerVote { get; }

        public RankingSheetBuilder(XLWorkbook workbook, JamTallyNewResult tallyResult) : base(workbook.AddWorksheet("Ranking"))
        {
            TallyResult = tallyResult;

            HeaderRows = new SheetSpan(1, 1);
            EntriesRows = new SheetSpan(HeaderRows, tallyResult.Ranking.Count);
            FooterRows = new SheetSpan(EntriesRows, 1);

            EntriesColumns = new SheetSpan(1, 2);
            ScoreColumns = new SheetSpan(EntriesColumns, 3);
            AwardColumns = new SheetSpan(ScoreColumns, tallyResult.AwardCriteria.Count);

            ColumnsPerVote = 2;
            VoteColumns = new SheetSpan(AwardColumns, ColumnsPerVote * tallyResult.Votes.Count);
        }

        // ---------
        // Addresses
        // ---------

        public string GetCurrentTitleAddress()
        {
            return Worksheet.Cell(CurrentRow, EntriesColumns.From).GetFixedColumnAddress();
        }

        public string GetEntriesRangeAddress()
        {
            return Range(EntriesRows, EntriesColumns).GetAbsoluteAddress();
        }

        // -------
        // Filling
        // -------

        public void ApplyStyle()
        {
            Worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            Worksheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            Worksheet.Style.Font.FontName = "Arial";
            Worksheet.Style.Font.FontSize = 10;

            Worksheet.SheetView.Freeze(HeaderRows.To, EntriesColumns.To);

            Worksheet.Row(HeaderRows.From).Style.Font.Bold = true;

            // entries
            DrawBox(HeaderRows, EntriesColumns, TallyStyles.RankHeader);
            DrawBox(EntriesRows, EntriesColumns, TallyStyles.RankBody);

            // score
            var mainScoreColumns = new SheetSpan(EntriesColumns, 1);
            var baseScoreColumns = new SheetSpan(mainScoreColumns, 1);
            var subScoreColumns = new SheetSpan(mainScoreColumns, 2);
            DrawBox(HeaderRows, mainScoreColumns, TallyStyles.ScoreHeader);
            DrawBox(EntriesRows, mainScoreColumns, TallyStyles.ScoreBodyStrong);
            DrawBox(HeaderRows, subScoreColumns, TallyStyles.ScoreHeader);
            DrawBox(EntriesRows, subScoreColumns, TallyStyles.ScoreBody);

            Range(EntriesRows, mainScoreColumns).Style.NumberFormat.Format = "0.00000";
            Range(EntriesRows, baseScoreColumns).Style.NumberFormat.Format = "0.00000";
            Range(EntriesRows, mainScoreColumns).Style.Font.Bold = true;

            // awards
            DrawBox(HeaderRows, AwardColumns, TallyStyles.AwardHeader);
            DrawBox(EntriesRows, AwardColumns, TallyStyles.AwardBody);

            // votes
            DrawBox(HeaderRows, VoteColumns, TallyStyles.VoteHeader);
            DrawBox(EntriesRows, VoteColumns, TallyStyles.VoteBody);

            for (var i = 0; i < TallyResult.Votes.Count; i++)
            {
                var voteScoreColumns = new SheetSpan(AwardColumns, ColumnsPerVote * i + 1, 1);
                var voteColumns = new SheetSpan(AwardColumns, ColumnsPerVote * i, ColumnsPerVote);
                Range(EntriesRows, voteScoreColumns).Style.NumberFormat.Format = "0.000";

                if (i % 2 == 1)
                    Range(EntriesRows, voteColumns).FillWith(TallyStyles.VoteBodyAlt);
            }
        }

        public void AddEntriesRegion()
        {
            Enter("GAME", "AUTHORS");
            foreach (var entry in TallyResult.Ranking)
            {
                Enter(entry.Title, entry.Authors);
            }

            CurrentRow = 1;
            CurrentColumn += EntriesColumns.Length;
        }

        public void AddScoreRegion(VotesSheetBuilder votesSheet)
        {
            Enter("SCORE", "BASE", "UJ");

            var totalVotesAddress = Worksheet.Cell(FooterRows.From, CurrentColumn + 1).GetAbsoluteAddress();

            foreach (var entry in TallyResult.Ranking)
            {
                var baseAddress = Worksheet.Cell(CurrentRow, CurrentColumn + 1).GetFixedColumnAddress();
                var unjudgedAddress = Worksheet.Cell(CurrentRow, CurrentColumn + 2).GetFixedColumnAddress();
                var totalScoreFormula = $"={baseAddress}*{totalVotesAddress}/({totalVotesAddress}-{unjudgedAddress})";

                var votesAddress = Range(CurrentRow, VoteColumns.From, CurrentRow, VoteColumns.To).GetFixedColumnAddress();
                var baseScoreFormula = $"=ARRAYFORMULA(SUMPRODUCT(mod(column({votesAddress}),2),{votesAddress}))";

                var titleAddress = GetCurrentTitleAddress();
                var unjudgedVotesAddress = votesSheet.GetUnjudgedRangeAddress();
                var unjudgedFormula = $"=IF(COUNTIF(Votes!{unjudgedVotesAddress},\"=\"&{titleAddress})>0,COUNTIF(Votes!{unjudgedVotesAddress},\"=\"&{titleAddress}),)";

                Enter(totalScoreFormula, baseScoreFormula, unjudgedFormula);
            }

            var unjudgedTotalColumns = new SheetSpan(EntriesColumns, 2, 1);
            var unjudgedTotalAddress = Range(EntriesRows, unjudgedTotalColumns).GetAbsoluteAddress();
            var unjudgedTotalFormula = $"=SUM({unjudgedTotalAddress})";

            CurrentColumn += 1;
            Enter(TallyResult.Votes.Count, unjudgedTotalFormula);
            CurrentColumn -= 1;

            CurrentRow = 1;
            CurrentColumn += ScoreColumns.Length;
        }

        public void AddAwardsRegion(VotesSheetBuilder votesSheet)
        {
            var criterionIndex = 0;
            foreach (var criterion in TallyResult.AwardCriteria)
            {
                Enter(criterion.Abbreviation);

                foreach (var entry in TallyResult.Ranking)
                {
                    AddEntryAward(criterionIndex, entry, votesSheet);
                }

                CurrentRow = 1;
                CurrentColumn += 1;
                criterionIndex += 1;
            }
        }

        private void AddEntryAward(int criterionIndex, JamTallyEntry entry, VotesSheetBuilder votesSheet)
        {
            var entryTitleAddress = GetCurrentTitleAddress();
            var awardRangeAddress = votesSheet.GetAwardRangeAddress(criterionIndex);

            var awardFormula = $"=IF(COUNTIF(Votes!{awardRangeAddress},\"=\"&{entryTitleAddress})>0,COUNTIF(Votes!{awardRangeAddress},\"=\"&{entryTitleAddress}),)";
            var enterRange = Enter(awardFormula);

            var criterion = TallyResult.AwardCriteria.ElementAt(criterionIndex);
            var awardScore = entry.AwardScores.FirstOrDefault(score => score.Criterion == criterion);
            if (awardScore != null && awardScore.Score == TallyResult.AwardTopScores[criterion])
            {
                enterRange.FillWith(TallyStyles.AwardBodyStrong);
                enterRange.Style.Font.Bold = true;
            }
        }

        public void AddVoteRegion(JamTallyVote vote, VotesSheetBuilder votesSheet)
        {
            var voteColumns = new SheetSpan(CurrentColumn, ColumnsPerVote);
            Enter(vote.Voter);
            Range(HeaderRows, voteColumns).Merge();

            var voteIndex = (CurrentColumn - VoteColumns.From) / ColumnsPerVote;
            var rankRangeAddress = votesSheet.GetRanksRangeAddress();
            var voteRangeAddress = votesSheet.GetVoteRangeAddress(voteIndex);

            foreach (var entry in TallyResult.Ranking)
            {
                var entryTitleAddress = GetCurrentTitleAddress();
                var voteRankAddress = Worksheet.Cell(CurrentRow, CurrentColumn).GetRelativeAddress();

                var rankFormula = $"=IFNA(VLOOKUP({entryTitleAddress},{{Votes!{voteRangeAddress},Votes!{rankRangeAddress}}},2,FALSE),)";
                var scoreFormula = $"=IF({voteRankAddress}>0,1/({voteRankAddress}+1),)";
                Enter(rankFormula, scoreFormula);
            }

            CurrentRow = 1;
            CurrentColumn += ColumnsPerVote;
        }
    }
}
