using System;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Result;
using ClosedXML.Excel;

namespace Alphicsh.JamTally.Spreadsheets
{
    public class AlignmentVotesSheetBuilder : SheetBuilder
    {
        private JamTallyAlignmentResult TallyResult { get; }

        public SheetSpan TitleColumns { get; }
        public SheetSpan VoteColumns { get; }
        int ColumnsPerVote { get; }

        public SheetSpan HeaderRows { get; }
        public SheetSpan RankingRows { get; }
        public SheetSpan UnjudgedRows { get; }

        public AlignmentVotesSheetBuilder(XLWorkbook workbook, JamTallyAlignmentResult tallyResult) : base(workbook.AddWorksheet("Color Votes"))
        {
            TallyResult = tallyResult;

            TitleColumns = new SheetSpan(1, 1);
            ColumnsPerVote = 2;
            VoteColumns = new SheetSpan(TitleColumns, ColumnsPerVote * tallyResult.Votes.Count);

            HeaderRows = new SheetSpan(1, 1);
            RankingRows = new SheetSpan(HeaderRows, tallyResult.Votes.Max(vote => vote.Ranking.Count));
            UnjudgedRows = new SheetSpan(RankingRows, Math.Max(1, tallyResult.Votes.Max(vote => vote.Unjudged.Count)));
        }

        // ---------
        // Addresses
        // ---------

        public string GetVoteRangeAddress(int voteIndex)
        {
            var voteTitleColumns = new SheetSpan(TitleColumns, ColumnsPerVote * voteIndex, 1);
            return Range(RankingRows, voteTitleColumns).GetAbsoluteAddress();
        }

        public string GetRanksRangeAddress()
        {
            return Range(RankingRows, TitleColumns).GetAbsoluteAddress();
        }

        public string GetAwardRangeAddress(int criterionIndex)
        {
            var awardRows = new SheetSpan(RankingRows, criterionIndex, 1);
            return Range(awardRows, VoteColumns).GetAbsoluteAddress();
        }

        public string GetUnjudgedRangeAddress()
        {
            return Range(UnjudgedRows, VoteColumns).GetAbsoluteAddress();
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

            Worksheet.SheetView.Freeze(HeaderRows.To, TitleColumns.To);

            Worksheet.Row(HeaderRows.From).Style.Font.Bold = true;
            Worksheet.Column(TitleColumns.From).Style.Font.Bold = true;

            DrawBox(HeaderRows, TitleColumns, TallyStyles.RankHeader);
            DrawBox(RankingRows, TitleColumns, TallyStyles.RankBody);

            DrawBox(HeaderRows, VoteColumns, TallyStyles.VoteHeader);
            DrawBox(RankingRows, VoteColumns, TallyStyles.VoteBody);

            DrawBox(UnjudgedRows, TitleColumns, TallyStyles.UnjudgedHeader);
            DrawBox(UnjudgedRows, VoteColumns, TallyStyles.UnjudgedBody);
        }

        public void AddLabelRegion()
        {
            CurrentRow = HeaderRows.From;
            Enter("RANK");

            CurrentRow = RankingRows.From;
            for (var i = 1; i <= RankingRows.Length; i++)
            {
                Enter(i);
            }

            CurrentRow = UnjudgedRows.From;
            Enter("Unjudged entries");
            Range(UnjudgedRows, TitleColumns).Merge();

            CurrentRow = 1;
            CurrentColumn += TitleColumns.Length;
        }

        public void AddVoteRegion(JamTallyVote vote, RankingSheetBuilder rankingSheet)
        {
            CurrentRow = HeaderRows.From;
            var voteColumns = new SheetSpan(CurrentColumn, ColumnsPerVote);
            Enter(vote.Voter);
            Range(HeaderRows, voteColumns).Merge();

            CurrentRow = RankingRows.From;
            foreach (var entry in vote.Ranking)
            {
                EnterVoteEntry(entry, rankingSheet);
            }

            CurrentRow = UnjudgedRows.From;
            foreach (var unjudged in vote.Unjudged)
            {
                EnterVoteEntry(unjudged, rankingSheet);
            }

            CurrentRow = 1;
            CurrentColumn += ColumnsPerVote;
        }

        private void EnterVoteEntry(JamEntry? entry, RankingSheetBuilder rankingSheet)
        {
            var title = entry?.TallyTitle ?? "";
            var titleAddress = Worksheet.Cell(CurrentRow, CurrentColumn).GetRelativeAddress();
            var entriesRangeAddress = rankingSheet.GetEntriesRangeAddress();
            var authorsFormula = $"=IF({titleAddress}=\"\",,VLOOKUP({titleAddress},Ranking!{entriesRangeAddress},2,FALSE))";
            Enter(title, authorsFormula);
        }
    }
}
