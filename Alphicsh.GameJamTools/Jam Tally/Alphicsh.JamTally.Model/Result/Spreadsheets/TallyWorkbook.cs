using System.IO;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Result.Alignments;
using Alphicsh.JamTally.Model.Result.Spreadsheets.Alignment;
using Alphicsh.JamTally.Model.Result.Spreadsheets.Ranking;
using Alphicsh.JamTally.Model.Result.Spreadsheets.Votes;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets
{
    internal class TallyWorkbook
    {
        public JamOverview Jam { get; }
        public JamTallyResult TallyResult { get; }
        public JamAlignmentTally? AlignmentTally { get; }
        public bool HasAlignments => Jam.Alignments != null;

        public RankingSheet RankingSheet { get; }
        public VotesSheet VotesSheet { get; }
        public AlignmentSheet? AlignmentSheet { get; }

        public TallyWorkbook(JamOverview jam, JamTallyResult tallyResult, JamAlignmentTally? alignmentTally)
        {
            Jam = jam;
            TallyResult = tallyResult;
            AlignmentTally = alignmentTally;

            RankingSheet = new RankingSheet(this);
            VotesSheet = new VotesSheet(this);
            if (HasAlignments)
                AlignmentSheet = new AlignmentSheet(this);
        }

        public void Populate()
        {
            RankingSheet.Populate();
            VotesSheet.Populate();

            if (HasAlignments)
                AlignmentSheet!.Populate();
        }

        public void Save(FilePath directoryPath)
        {
            var rankingPath = directoryPath.Append("Ranking.csv");
            var rankingContent = RankingSheet.GenerateCsv();
            File.WriteAllText(rankingPath.Value, rankingContent);

            var votesPath = directoryPath.Append("Votes.csv");
            var votesContent = VotesSheet.GenerateCsv();
            File.WriteAllText(votesPath.Value, votesContent);

            if (HasAlignments)
            {
                var alignmentPath = directoryPath.Append("Alignments.csv");
                var alignmentContent = AlignmentSheet!.GenerateCsv();
                File.WriteAllText(alignmentPath.Value, alignmentContent);
            }
        }
    }
}
