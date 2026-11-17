using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Result.Export;
using Alphicsh.JamTally.Spreadsheets;
using Alphicsh.JamTally.Trophies.Image.Generators;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTally.Model.Result
{
    public class JamTallyResultManager
    {
        public JamTallyResult Result { get; init; } = default!;

        // -------
        // Ranking
        // -------

        public string GetFinalRankingText()
            => string.Join("\n", Result.Ranking.Select(entry => entry.ToString()));

        // ------
        // Awards
        // ------

        public string GetAwardRankingsText()
        {
            var lines = Result.AwardCriteria.SelectMany(criterion => GetAwardRankingLines(criterion, Result.AwardEntryScores[criterion]));
            return string.Join("\n", lines);
        }
        private IEnumerable<string> GetAwardRankingLines(JamAwardCriterion criterion, IReadOnlyList<JamTallyAwardScore> awardScores)
        {
            yield return criterion.Name + ":";

            foreach (var score in awardScores)
                yield return $"{score.Entry.Line}: {score.Score}";

            yield return "";
        }

        // ----------
        // Generators
        // ----------

        private static JamTallyResultExporter ResultsJsonExporter { get; } = new JamTallyResultExporter();

        public void ExportResultsJson(FilePath directoryPath)
        {
            ResultsJsonExporter.ExportResult(directoryPath, Result);
        }

        private static TallySpreadsheetExporter TallySpreadsheetExporter { get; } = new TallySpreadsheetExporter();

        public void GenerateTallySheets(FilePath exportPath)
        {
            TallySpreadsheetExporter.Export(exportPath, Result);
        }

        private static ResultsPostGenerator ResultsPostGenerator { get; } = new ResultsPostGenerator();

        public string GenerateResultsPost()
            => ResultsPostGenerator.Generate(Result);

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

            var image = generator.Generate(document, settings, Result);
            File.WriteAllText(destinationPath.Value, image.Format());
        }

        public void CompileTrophies(FilePath sourcePath, FilePath destinationPath)
        {
            EnsureDifferentSavePath(sourcePath, destinationPath);
            var generator = new TrophiesAssembler();
            var settings = new TrophiesImageSettings { TrophyWidth = 540, TrophyHeight = 120, ColumnWidth = 600, RowHeight = 140 };
            var document = XDocument.Load(sourcePath.Value);

            var image = generator.AssembleTrophies(document, settings, Result);
            File.WriteAllText(destinationPath.Value, image.Format());
        }

        private void EnsureDifferentSavePath(FilePath sourcePath, FilePath destinationPath)
        {
            if (sourcePath == destinationPath)
                throw new InvalidOperationException("The source file path must be different than the destination path.");
        }
    }
}
