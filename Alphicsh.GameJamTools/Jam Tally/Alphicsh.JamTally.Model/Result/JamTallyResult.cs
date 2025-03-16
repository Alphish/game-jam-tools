using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Result.Alignments;
using Alphicsh.JamTally.Model.Result.Trophies.Image;
using Alphicsh.JamTally.Model.Vote;
using Alphicsh.JamTally.Spreadsheets;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTally.Model.Result
{
    public class JamTallyResult
    {
        public IReadOnlyCollection<JamAwardCriterion> Awards { get; init; } = default!;
        public IReadOnlyCollection<JamEntry> Entries { get; init; } = default!;
        public IReadOnlyCollection<JamVote> Votes { get; init; } = default!;

        public int EntriesCount { get; init; }
        public int AwardsCount { get; init; }
        public int UnjudgedMaxCount { get; init; }
        public int ReactionsMaxCount { get; init; }

        public JamAlignmentTally? AlignmentTally { get; init; }

        public JamVoteCollection VoteCollection { get; set; } = default!;

        // -------
        // Ranking
        // -------

        public IReadOnlyCollection<JamTallyEntryScore> FinalRanking { get; init; } = default!;
        public string GetFinalRankingText()
            => string.Join("\n", FinalRanking.Select(score => score.ToString()));

        // ------
        // Awards
        // ------

        public IReadOnlyCollection<JamTallyAwardRanking> AwardRankings { get; init; } = default!;
        public string GetAwardRankingsText()
        {
            var lines = AwardRankings.SelectMany(ranking => GetAwardRankingLines(ranking));
            return string.Join("\n", lines);
        }
        private IEnumerable<string> GetAwardRankingLines(JamTallyAwardRanking ranking)
        {
            yield return ranking.Award.Name + ":";

            foreach (var score in ranking.Scores)
                yield return $"{score.Entry.Line}: {score.Count}";

            yield return "";
        }

        // ----------
        // Generators
        // ----------

        private static TallySpreadsheetExporter TallySpreadsheetExporter { get; } = new TallySpreadsheetExporter();

        public void GenerateTallySheets(FilePath exportPath)
        {
            TallySpreadsheetExporter.Export(exportPath, VoteCollection.NewTallyResult!);
        }

        private static ResultsPostGenerator ResultsPostGenerator { get; } = new ResultsPostGenerator();

        public string GenerateResultsPost()
            => ResultsPostGenerator.Generate(this);

        private static TrophiesImageGenerator TrophiesImageGenerator { get; } = new TrophiesImageGenerator();

        public void GenerateTrophiesCoreTemplate(FilePath sourcePath, FilePath destinationPath)
            => TrophiesImageGenerator.GenerateCoreTemplate(sourcePath, destinationPath);

        public void GenerateTrophiesEntriesTemplate(FilePath sourcePath, FilePath destinationPath)
            => TrophiesImageGenerator.GenerateEntriesTemplate(sourcePath, destinationPath);

        public void CompileTrophies(FilePath sourcePath, FilePath destinationPath)
            => TrophiesImageGenerator.CompileTrophies(tallyResult: this, sourcePath, destinationPath);
    }
}
