using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Result.Alignments;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets.Alignment
{
    internal class AlignmentSheet : Sheet
    {
        public TallyWorkbook Workbook { get; }
        public JamOverview Jam { get; }
        public JamTallyResult TallyResult { get; }
        public JamAlignmentTally AlignmentTally { get; }

        public int EntriesCount { get; }
        public int AlignmentsCount { get; }

        public int SummaryHeaderRow { get; }
        public int SummaryAlignmentFirstRow { get; }
        public int SummaryAlignmentLastRow => SummaryAlignmentFirstRow + AlignmentsCount - 1;
        public int SummaryAlignmentNameColumn { get; }
        public int SummaryAlignmentBaseScoreColumn { get; }
        public int SummaryAlignmentReviewMultiplierColumn { get; }
        public int SummaryAlignmentThemeBonusColumn { get; }
        public int SummaryAlignmentOverallColumn { get; }

        public int ReviewsHeaderRow { get; }
        public int ReviewsAlignmentFirstRow { get; }
        public int ReviewsAlignmentLastRow => ReviewsAlignmentFirstRow + AlignmentsCount - 1;
        public int ReviewsAlignmentNameColumn { get; }
        public int ReviewsAlignmentEligibleVotesColumn { get; }
        public int ReviewsAlignmentVotelessEntriesColumn { get; }
        public int ReviewsAlignmentMultiplierColumn { get; }
        
        public int ReviewsEntriesFirstRow { get; }
        public int ReviewsEntriesLastRow => ReviewsEntriesFirstRow + EntriesCount - 1;
        public int ReviewsEntriesFirstColumn { get; }
        public int ReviewsEntriesWidth { get; }
        public int ReviewsEntriesLastColumn => ReviewsEntriesFirstColumn + ReviewsEntriesWidth - 1;
        public int ReviewsEntriesVoteStatusColumn { get; }

        public int VoteWidth { get; }
        public int VotesCount { get; }
        public int VotesAlignmentColumn { get; }
        public int VotesAlignmentTotalColumn { get; }
        public int VotesFirstColumn { get; }
        public int VotesLastColumn => VotesFirstColumn + VoteWidth * VotesCount - 1;

        public int VotesHeaderRow { get; }
        public int VotesAlignmentFirstRow { get; }
        public int VotesAlignmentLastRow => VotesAlignmentFirstRow + AlignmentsCount - 1;
        public IReadOnlyDictionary<JamAlignmentOption, int> VotesAlignmentRows { get; }
        public IReadOnlyDictionary<JamVote, int> VotesColumns { get; }

        public int RankingFirstRow { get; }
        public int RankingLastRow => RankingFirstRow + EntriesCount - 1;

        public int MaxUnrankedCount { get; }
        public int UnrankedFirstRow { get; }
        public int UnrankedLastRow => UnrankedFirstRow + MaxUnrankedCount - 1;

        public int TotalWidth => VotesLastColumn;
        public int TotalHeight => UnrankedLastRow;

        public AlignmentSheet(TallyWorkbook workbook)
        {
            Workbook = workbook;
            Jam = Workbook.Jam;
            TallyResult = workbook.TallyResult;
            AlignmentTally = TallyResult.AlignmentTally!;

            EntriesCount = Jam.Entries.Count;
            AlignmentsCount = Jam.Alignments!.GetAvailableOptions().Count();

            SummaryHeaderRow = 1;
            SummaryAlignmentFirstRow = SummaryHeaderRow + 1;
            SummaryAlignmentNameColumn = 1;
            SummaryAlignmentBaseScoreColumn = SummaryAlignmentNameColumn + 1;
            SummaryAlignmentReviewMultiplierColumn = SummaryAlignmentBaseScoreColumn + 1;
            SummaryAlignmentThemeBonusColumn = SummaryAlignmentReviewMultiplierColumn + 1;
            SummaryAlignmentOverallColumn = SummaryAlignmentThemeBonusColumn + 1;

            ReviewsHeaderRow = SummaryAlignmentLastRow + 2;
            ReviewsAlignmentFirstRow = ReviewsHeaderRow + 1;
            ReviewsAlignmentNameColumn = 1;
            ReviewsAlignmentEligibleVotesColumn = ReviewsAlignmentNameColumn + 1;
            ReviewsAlignmentVotelessEntriesColumn = ReviewsAlignmentEligibleVotesColumn + 1;
            ReviewsAlignmentMultiplierColumn = ReviewsAlignmentVotelessEntriesColumn + 1;

            ReviewsEntriesFirstRow = ReviewsAlignmentLastRow + 2;
            ReviewsEntriesFirstColumn = ReviewsAlignmentNameColumn;
            ReviewsEntriesVoteStatusColumn = ReviewsEntriesLastColumn + 1;

            VoteWidth = 6;
            VotesCount = AlignmentTally.Votes.Count;
            VotesAlignmentColumn = ReviewsAlignmentMultiplierColumn + 2;
            VotesAlignmentTotalColumn = VotesAlignmentColumn + 1;
            VotesFirstColumn = VotesAlignmentTotalColumn + 1;

            VotesHeaderRow = ReviewsHeaderRow;
            VotesAlignmentFirstRow = VotesHeaderRow + 1;
            VotesAlignmentRows = Jam.Alignments!.GetAvailableOptions()
                .Select((option, i) => new KeyValuePair<JamAlignmentOption, int>(option, VotesAlignmentFirstRow + 1))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            VotesColumns = TallyResult.Votes
                .Select((vote, i) => new KeyValuePair<JamVote, int>(vote, VotesFirstColumn + VoteWidth * i))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            RankingFirstRow = VotesAlignmentLastRow + 2;
            MaxUnrankedCount = TallyResult.Votes.Max(vote => vote.Missing.Count);
            UnrankedFirstRow = RankingLastRow + 1;

            ExpandTo(TotalWidth, TotalHeight);
        }

        // ----------
        // Populating
        // ----------

        public void Populate()
        {
            foreach (var vote in AlignmentTally.Votes)
            {
                var voteArea = new AlignmentVoteArea(this, vote);
                voteArea.Populate();
            }

            var baseScoreArea = new AlignmentBaseScoreArea(this);
            baseScoreArea.Populate();

            var reviewsArea = new AlignmentReviewsArea(this);
            reviewsArea.Populate();

            var summaryArea = new AlignmentSummaryArea(this);
            summaryArea.Populate();
        }
    }
}
