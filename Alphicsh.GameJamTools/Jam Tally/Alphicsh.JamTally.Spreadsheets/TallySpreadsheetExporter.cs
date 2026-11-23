using Alphicsh.JamTally.Model.Result;
using Alphicsh.JamTools.Common.IO;
using ClosedXML.Excel;

namespace Alphicsh.JamTally.Spreadsheets
{
    public class TallySpreadsheetExporter
    {
        public void Export(FilePath path, JamTallyResult result)
        {
            var workbook = GenerateWorkbook(result);
            workbook.SaveAs(path.Value);
        }

        private XLWorkbook GenerateWorkbook(JamTallyResult result)
        {
            var workbook = new XLWorkbook();

            var rankingBuilder = new RankingSheetBuilder(workbook, result);
            var votesBuilder = new VotesSheetBuilder(workbook, result);

            // populating Ranking sheet
            rankingBuilder.ApplyStyle();
            rankingBuilder.AddEntriesRegion();
            rankingBuilder.AddScoreRegion(votesBuilder);
            rankingBuilder.AddAwardsRegion(votesBuilder);
            foreach (var vote in result.Votes)
            {
                rankingBuilder.AddVoteRegion(vote, votesBuilder);
            }

            // populating Votes sheet
            votesBuilder.ApplyStyle();
            votesBuilder.AddLabelRegion();
            foreach (var vote in result.Votes)
            {
                votesBuilder.AddVoteRegion(vote, rankingBuilder);
            }

            if (result.AlignmentResult != null)
            {
                var alignmentRankingBuilder = new AlignmentRankingSheetBuilder(workbook, result.AlignmentResult);
                var alignmentVotesBuilder = new AlignmentVotesSheetBuilder(workbook, result.AlignmentResult);

                alignmentRankingBuilder.ApplyStyle();
                alignmentRankingBuilder.AddAlignmentRegion();
                alignmentRankingBuilder.AddEntriesRegion();
                alignmentRankingBuilder.AddScoreRegion(alignmentVotesBuilder);
                foreach (var vote in result.Votes)
                {
                    alignmentRankingBuilder.AddVoteRegion(vote, alignmentVotesBuilder);
                }

                alignmentVotesBuilder.ApplyStyle();
                alignmentVotesBuilder.AddLabelRegion();
                foreach (var vote in result.AlignmentResult.Votes)
                {
                    alignmentVotesBuilder.AddVoteRegion(vote, rankingBuilder);
                }
            }

            return workbook;
        }
    }
}
