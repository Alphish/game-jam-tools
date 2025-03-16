using Alphicsh.JamTally.Model.Result;
using Alphicsh.JamTools.Common.IO;
using ClosedXML.Excel;

namespace Alphicsh.JamTally.Spreadsheets
{
    public class TallySpreadsheetExporter
    {
        public void Export(FilePath path, JamTallyNewResult result)
        {
            var workbook = GenerateWorkbook(result);
            workbook.SaveAs(path.Value);
        }

        private XLWorkbook GenerateWorkbook(JamTallyNewResult result)
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

            return workbook;
        }
    }
}
