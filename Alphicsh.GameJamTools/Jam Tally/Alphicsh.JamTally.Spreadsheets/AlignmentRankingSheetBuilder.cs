using Alphicsh.JamTally.Model.Result;
using ClosedXML.Excel;

namespace Alphicsh.JamTally.Spreadsheets
{
    public class AlignmentRankingSheetBuilder : SheetBuilder
    {
        private JamTallyAlignmentResult TallyResult { get; }

        public SheetSpan HeaderRows { get; }
        public SheetSpan AlignmentsRows { get; }
        public SheetSpan SeparatorRows { get; }
        public SheetSpan EntryHeaderRows { get; }
        public SheetSpan EntriesRows { get; }
        public SheetSpan FooterRows { get; }

        public SheetSpan EntriesColumns { get; }
        public SheetSpan ScoreColumns { get; }
        public SheetSpan ScoreComponentColumns { get; }
        public SheetSpan VoteColumns { get; }
        public int ColumnsPerVote { get; }

        public AlignmentRankingSheetBuilder(XLWorkbook workbook, JamTallyAlignmentResult tallyResult) : base(workbook.AddWorksheet("Color Ranking"))
        {
            TallyResult = tallyResult;

            HeaderRows = new SheetSpan(1, 1);
            AlignmentsRows = new SheetSpan(HeaderRows, tallyResult.Ranking.Count);
            SeparatorRows = new SheetSpan(AlignmentsRows, 1);
            EntryHeaderRows = new SheetSpan(SeparatorRows, 1);
            EntriesRows = new SheetSpan(EntryHeaderRows, tallyResult.Entries.Count);
            FooterRows = new SheetSpan(EntriesRows, 1);

            EntriesColumns = new SheetSpan(1, 3);
            ScoreColumns = new SheetSpan(EntriesColumns, 1);
            ScoreComponentColumns = new SheetSpan(ScoreColumns, 4);

            ColumnsPerVote = 2;
            VoteColumns = new SheetSpan(ScoreComponentColumns, ColumnsPerVote * tallyResult.Votes.Count);
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

            Worksheet.SheetView.Freeze(0, EntriesColumns.To);

            Worksheet.Row(HeaderRows.From).Style.Font.Bold = true;
            Worksheet.Row(EntryHeaderRows.From).Style.Font.Bold = true;

            var baseScoreColumns = new SheetSpan(ScoreColumns, 1);
            var multiplierColumns = new SheetSpan(baseScoreColumns, 1);

            // alignments
            Range(AlignmentsRows, ScoreColumns).Style.NumberFormat.Format = "0.00000";
            Range(AlignmentsRows, baseScoreColumns).Style.NumberFormat.Format = "0.00000";
            Range(AlignmentsRows, multiplierColumns).Style.NumberFormat.Format = "0.00";
            Range(AlignmentsRows, ScoreColumns).Style.Font.Bold = true;

            // entries
            DrawBox(EntryHeaderRows, EntriesColumns, TallyStyles.RankHeader);
            DrawBox(EntriesRows, EntriesColumns, TallyStyles.RankBody);

            // score
            DrawBox(EntryHeaderRows, ScoreColumns, TallyStyles.ScoreHeader);
            DrawBox(EntriesRows, ScoreColumns, TallyStyles.ScoreBodyStrong);
            DrawBox(EntryHeaderRows, ScoreComponentColumns, TallyStyles.ScoreHeader);
            DrawBox(EntriesRows, ScoreComponentColumns, TallyStyles.ScoreBody);

            Range(EntriesRows, ScoreColumns).Style.NumberFormat.Format = "0.00000";
            Range(EntriesRows, baseScoreColumns).Style.NumberFormat.Format = "0.00000";
            Range(EntriesRows, multiplierColumns).Style.NumberFormat.Format = "0.00";
            Range(EntriesRows, ScoreColumns).Style.Font.Bold = true;

            // votes
            DrawBox(EntryHeaderRows, VoteColumns, TallyStyles.VoteHeader);
            DrawBox(EntriesRows, VoteColumns, TallyStyles.VoteBody);

            for (var i = 0; i < TallyResult.Votes.Count; i++)
            {
                var voteScoreColumns = new SheetSpan(ScoreComponentColumns, ColumnsPerVote * i + 1, 1);
                var voteColumns = new SheetSpan(ScoreComponentColumns, ColumnsPerVote * i, ColumnsPerVote);
                Range(EntriesRows, voteScoreColumns).Style.NumberFormat.Format = "0.000";

                if (i % 2 == 1)
                    Range(EntriesRows, voteColumns).FillWith(TallyStyles.VoteBodyAlt);
            }
        }

        public void AddAlignmentRegion()
        {
            CurrentRow = HeaderRows.From;
            CurrentColumn = EntriesColumns.From;

            Enter("COLOR", "", "", "SCORE", "BASE", "MLT", "REV", "THE");
            Range(HeaderRows, EntriesColumns).Merge();

            foreach (var alignment in TallyResult.Ranking)
            {
                var baseAddress = Worksheet.Cell(CurrentRow, CurrentColumn + 4).GetFixedColumnAddress();
                var multiplierAddress = Worksheet.Cell(CurrentRow, CurrentColumn + 5).GetFixedColumnAddress();
                var reviewsAddress = Worksheet.Cell(CurrentRow, CurrentColumn + 6).GetFixedColumnAddress();
                var themeAddress = Worksheet.Cell(CurrentRow, CurrentColumn + 7).GetFixedColumnAddress();

                var colorColumns = new SheetSpan(EntriesColumns, -1, 1);
                var colorRange = Range(EntriesRows, colorColumns).GetAbsoluteAddress();
                var scoreRange = Range(EntriesRows, ScoreColumns).GetAbsoluteAddress();
                var reviewMarkRange = Range(FooterRows, VoteColumns).GetAbsoluteAddress();

                var scoreFormula = $"={baseAddress}*{multiplierAddress}";
                var baseFormula = $"=SUMIF({colorRange},\"{alignment.Alignment.ShortTitle}\",{scoreRange})";
                var multiplierFormula = $"=1.00+0.05*{reviewsAddress}+IF({themeAddress}=\"Y\",0.01,0.00)";
                var reviewsFormula = $"=COUNTIF({reviewMarkRange},\"{alignment.Alignment.ShortTitle} (full)\")";

                Enter(alignment.Alignment.Title, "", "", scoreFormula, baseFormula, multiplierFormula, reviewsFormula, alignment.GuessedTheme ? "Y" : "");
                Range(CurrentRow - 1, EntriesColumns.From, CurrentRow - 1, EntriesColumns.To).Merge();
            }
        }

        public void AddEntriesRegion()
        {
            CurrentRow = EntryHeaderRows.From;
            CurrentColumn = EntriesColumns.From;

            Enter("GAME", "AUTHORS", "COLOR");
            foreach (var entry in TallyResult.Entries)
            {
                Enter(entry.Title, entry.Authors, entry.Alignment!.ShortTitle);
            }
        }

        public void AddScoreRegion(AlignmentVotesSheetBuilder votesSheet)
        {
            CurrentRow = EntryHeaderRows.From;
            CurrentColumn = ScoreColumns.From;

            Enter("SCORE", "BASE", "MULT", "JDG", "UJ");

            foreach (var entry in TallyResult.Entries)
            {
                var baseAddress = Worksheet.Cell(CurrentRow, CurrentColumn + 1).GetFixedColumnAddress();
                var multiplierAddress = Worksheet.Cell(CurrentRow, CurrentColumn + 2).GetFixedColumnAddress();
                var judgedAddress = Worksheet.Cell(CurrentRow, CurrentColumn + 3).GetFixedColumnAddress();
                var unjudgedAddress = Worksheet.Cell(CurrentRow, CurrentColumn + 4).GetFixedColumnAddress();

                var judgedFormula = $"={TallyResult.Votes.Count}-{unjudgedAddress}";
                var multiplierFormula = $"=({judgedAddress}+{unjudgedAddress})/{judgedAddress}";
                var totalScoreFormula = $"={baseAddress}*{multiplierAddress}";

                var votesAddress = Range(CurrentRow, VoteColumns.From, CurrentRow, VoteColumns.To).GetFixedColumnAddress();
                var baseScoreFormula = $"=ARRAYFORMULA(SUMPRODUCT(1-mod(column({votesAddress}),2),{votesAddress}))";

                var titleAddress = GetCurrentTitleAddress();
                var unjudgedVotesAddress = votesSheet.GetUnjudgedRangeAddress();
                var unjudgedFormula = $"=IF(COUNTIF('Color Votes'!{unjudgedVotesAddress},\"=\"&{titleAddress})>0,COUNTIF('Color Votes'!{unjudgedVotesAddress},\"=\"&{titleAddress}),)";

                Enter(totalScoreFormula, baseScoreFormula, multiplierFormula, judgedFormula, unjudgedFormula);
            }

            CurrentRow = EntryHeaderRows.From;
            CurrentColumn = VoteColumns.From;
        }

        public void AddVoteRegion(JamTallyVote vote, AlignmentVotesSheetBuilder votesSheet)
        {
            var voteColumns = new SheetSpan(CurrentColumn, ColumnsPerVote);
            Enter(vote.Voter);
            Range(EntryHeaderRows, voteColumns).Merge();

            var voteIndex = (CurrentColumn - VoteColumns.From) / ColumnsPerVote;
            var rankRangeAddress = votesSheet.GetRanksRangeAddress();
            var voteRangeAddress = votesSheet.GetVoteRangeAddress(voteIndex);

            foreach (var entry in TallyResult.Entries)
            {
                var entryTitleAddress = GetCurrentTitleAddress();
                var voteRankAddress = Worksheet.Cell(CurrentRow, CurrentColumn).GetRelativeAddress();

                var rankFormula = $"=IFNA(VLOOKUP({entryTitleAddress},{{'Color Votes'!{voteRangeAddress},'Color Votes'!{rankRangeAddress}}},2,FALSE),)";
                var scoreFormula = $"=IF({voteRankAddress}>0,1/({voteRankAddress}+3),)";
                Enter(rankFormula, scoreFormula);
            }

            var alignText = "Neutral";
            if (vote.VoterAlignment != null)
                alignText = vote.VoterAlignment.ShortTitle + " " + (vote.IsComplete ? "(full)" : "(partial)");

            Enter(alignText);
            Range(FooterRows, voteColumns).Merge();

            CurrentRow = EntryHeaderRows.From;
            CurrentColumn += ColumnsPerVote;
        }
    }
}
