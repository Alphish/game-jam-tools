using System;
using System.IO;
using System.Xml.Linq;
using Alphicsh.JamTally.Trophies.Image.Generators;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTally.Model.Result.Trophies.Image
{
    public class TrophiesImageGenerator
    {
        // ----
        // Core
        // ----

        public void GenerateCoreTemplate(FilePath destinationPath)
        {
            var generator = new TrophiesCoreGenerator();
            var settings = new TrophiesImageSettings { TrophyWidth = 540, TrophyHeight = 120, ColumnWidth = 600, RowHeight = 140 };
            var image = generator.Generate(settings);
            File.WriteAllText(destinationPath.Value, image.Format());
        }

        // -------
        // Entries
        // -------

        public void GenerateEntriesTemplate(FilePath sourcePath, JamTallyNewResult result, FilePath destinationPath)
        {
            EnsureDifferentSavePath(sourcePath, destinationPath);
            var generator = new TrophiesEntriesGenerator();
            var settings = new TrophiesImageSettings { TrophyWidth = 540, TrophyHeight = 120, ColumnWidth = 600, RowHeight = 140 };
            var document = XDocument.Load(sourcePath.Value);

            var image = generator.Generate(document, settings, result);
            File.WriteAllText(destinationPath.Value, image.Format());
        }

        // --------
        // Trophies
        // --------

        public void CompileTrophies(JamTallyResult tallyResult, FilePath sourcePath, FilePath destinationPath)
        {
            EnsureDifferentSavePath(sourcePath, destinationPath);
            var generator = new TrophiesAssembler();
            var settings = new TrophiesImageSettings { TrophyWidth = 540, TrophyHeight = 120, ColumnWidth = 600, RowHeight = 140 };
            var document = XDocument.Load(sourcePath.Value);

            var image = generator.AssembleTrophies(document, settings, tallyResult.VoteCollection.NewTallyResult!);
            File.WriteAllText(destinationPath.Value, image.Format());
        }

        private void EnsureDifferentSavePath(FilePath sourcePath, FilePath destinationPath)
        {
            if (sourcePath == destinationPath)
                throw new InvalidOperationException("This isn't very tested, so let's not overwrite things just yet.");
        }
    }
}
