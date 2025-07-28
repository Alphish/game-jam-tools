using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Result.Alignments;
using Alphicsh.JamTally.Model.Vote;
using Alphicsh.JamTally.Spreadsheets;
using Alphicsh.JamTally.Trophies.Image.Generators;
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
            => ResultsPostGenerator.Generate(VoteCollection.NewTallyResult!);

        public void GenerateTrophiesCoreTemplate(FilePath destinationPath)
        {
            var generator = new TrophiesCoreGenerator();
            var settings = new TrophiesImageSettings { TrophyWidth = 540, TrophyHeight = 120, ColumnWidth = 600, RowHeight = 140 };
            var image = generator.Generate(settings);
            File.WriteAllText(destinationPath.Value, image.Format());
        }

        public void GenerateTrophiesEntriesTemplate(FilePath sourcePath, FilePath destinationPath)
        {
            EnsureDifferentSavePath(sourcePath, destinationPath);
            var generator = new TrophiesEntriesGenerator();
            var settings = new TrophiesImageSettings { TrophyWidth = 540, TrophyHeight = 120, ColumnWidth = 600, RowHeight = 140 };
            var document = XDocument.Load(sourcePath.Value);

            var image = generator.Generate(document, settings, VoteCollection.NewTallyResult!);
            File.WriteAllText(destinationPath.Value, image.Format());
        }

        public void CompileTrophies(FilePath sourcePath, FilePath destinationPath)
        {
            EnsureDifferentSavePath(sourcePath, destinationPath);
            var generator = new TrophiesAssembler();
            var settings = new TrophiesImageSettings { TrophyWidth = 540, TrophyHeight = 120, ColumnWidth = 600, RowHeight = 140 };
            var document = XDocument.Load(sourcePath.Value);

            var image = generator.AssembleTrophies(document, settings, VoteCollection.NewTallyResult!);
            File.WriteAllText(destinationPath.Value, image.Format());
        }

        private void EnsureDifferentSavePath(FilePath sourcePath, FilePath destinationPath)
        {
            if (sourcePath == destinationPath)
                throw new InvalidOperationException("The source file path must be different than the destination path.");
        }
    }
}
