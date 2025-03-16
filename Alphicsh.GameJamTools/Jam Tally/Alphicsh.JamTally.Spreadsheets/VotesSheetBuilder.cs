using System;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Result;
using ClosedXML.Excel;

namespace Alphicsh.JamTally.Spreadsheets
{
    public class VotesSheetBuilder : SheetBuilder
    {
        private JamTallyNewResult TallyResult { get; }

        public SheetSpan TitleColumns { get; }
        public SheetSpan VoteColumns { get; }
        int ColumnsPerVote { get; }

        public SheetSpan HeaderRows { get; }
        public SheetSpan RankingRows { get; }
        public SheetSpan AwardRows { get; }
        public SheetSpan UnjudgedRows { get; }
        public SheetSpan ReactionRows { get; }
        public SheetSpan ReactionTotalsRows { get; }
        public SheetSpan ReactionTitleRows { get; }

        public VotesSheetBuilder(XLWorkbook workbook, JamTallyNewResult tallyResult) : base(workbook.AddWorksheet("Votes"))
        {
            TallyResult = tallyResult;

            TitleColumns = new SheetSpan(1, 1);
            ColumnsPerVote = 2;
            VoteColumns = new SheetSpan(TitleColumns, ColumnsPerVote * tallyResult.Votes.Count);

            HeaderRows = new SheetSpan(1, 1);
            RankingRows = new SheetSpan(HeaderRows, tallyResult.Ranking.Count);
            AwardRows = new SheetSpan(RankingRows, tallyResult.AwardCriteria.Count);
            UnjudgedRows = new SheetSpan(AwardRows, Math.Max(1, tallyResult.Votes.Max(vote => vote.Unjudged.Count)));
            ReactionRows = new SheetSpan(UnjudgedRows, Math.Max(1, tallyResult.Votes.Max(vote => vote.Reactions.Count)));
            ReactionTotalsRows = new SheetSpan(ReactionRows, 1);
            ReactionTitleRows = new SheetSpan(ReactionRows.From, ReactionRows.Length + ReactionTotalsRows.Length);
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
            Worksheet.Row(ReactionTotalsRows.From).Style.Font.Bold = true;
            Worksheet.Column(TitleColumns.From).Style.Font.Bold = true;

            DrawBox(HeaderRows, TitleColumns, TallyStyles.RankHeader);
            DrawBox(RankingRows, TitleColumns, TallyStyles.RankBody);

            DrawBox(HeaderRows, VoteColumns, TallyStyles.VoteHeader);
            DrawBox(RankingRows, VoteColumns, TallyStyles.VoteBody);

            DrawBox(AwardRows, TitleColumns, TallyStyles.AwardHeader);
            DrawBox(AwardRows, VoteColumns, TallyStyles.AwardBodyStrong);

            DrawBox(UnjudgedRows, TitleColumns, TallyStyles.UnjudgedHeader);
            DrawBox(UnjudgedRows, VoteColumns, TallyStyles.UnjudgedBody);

            var reactionScoreTitleRows = new SheetSpan(UnjudgedRows, ReactionRows.Length + ReactionTotalsRows.Length);
            DrawBox(reactionScoreTitleRows, TitleColumns, TallyStyles.ScoreHeader);
            DrawBox(ReactionRows, VoteColumns, TallyStyles.ScoreBody);
            DrawBox(ReactionTotalsRows, VoteColumns, TallyStyles.ScoreBodyStrong);
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

            CurrentRow = AwardRows.From;
            foreach (var award in TallyResult.AwardCriteria)
            {
                Enter(award.TallyName);
            }

            CurrentRow = UnjudgedRows.From;
            Enter("Unjudged entries");
            Range(UnjudgedRows, TitleColumns).Merge();

            CurrentRow = ReactionRows.From;
            Enter("Reactions");
            Range(ReactionTitleRows, TitleColumns).Merge();

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

            CurrentRow = AwardRows.From;
            foreach (var criterion in TallyResult.AwardCriteria)
            {
                if (vote.Awards.TryGetValue(criterion, out var entry))
                    EnterVoteEntry(entry, rankingSheet);
                else
                    EnterVoteEntry(null, rankingSheet);
            }

            CurrentRow = UnjudgedRows.From;
            foreach (var unjudged in vote.Unjudged)
            {
                EnterVoteEntry(unjudged, rankingSheet);
            }

            CurrentRow = ReactionRows.From;
            foreach (var reaction in vote.Reactions)
            {
                var reactionRange = Enter(reaction.Name, reaction.Value);
                if (reaction.Value > 2)
                {
                    reactionRange.FillWith(TallyStyles.ScoreBodyStrong);
                    reactionRange.Style.Font.Bold = true;
                }
            }

            CurrentRow = ReactionTotalsRows.From;
            var reactionTotalRange = Enter("Total", vote.ReviewScore);
            if (vote.ReviewScore == TallyResult.TopReviewScore)
                reactionTotalRange.FillWith(TallyStyles.ScoreHeader);

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
