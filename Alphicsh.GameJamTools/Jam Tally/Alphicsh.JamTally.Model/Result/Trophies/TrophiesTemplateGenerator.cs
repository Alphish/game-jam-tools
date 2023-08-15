using System.Xml.Linq;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTally.Model.Result.Trophies
{
    public class TrophiesTemplateGenerator
    {
        public void Generate(JamTallyResult tallyResult, FilePath sourcePath, FilePath destinationPath)
        {
            var trophies = LoadTrophies(sourcePath);
            var builder = new TrophiesBuilder(trophies, tallyResult);
            PopulateMassExportAreas(builder, tallyResult);
            PopulateExportAreas(builder);
            PopulateBoxes(builder);
            trophies.Save(destinationPath.Value);
        }

        private XDocument LoadTrophies(FilePath sourcePath)
        {
            return XDocument.Load(sourcePath.Value);
        }

        private void PopulateMassExportAreas(TrophiesBuilder builder, JamTallyResult tallyResult)
        {
            var layer = builder.Layers["MassExport"];
            layer.RemoveNodes();

            layer.Add(InkElements.CreateExportArea(
                "ranking_export", "e08020", 0, 0, 480, 140 * tallyResult.EntriesCount - 20, "ranking.png"
                ));

            layer.Add(InkElements.CreateExportArea(
                "top3_export", "e0a020", 0, 0, 480, 400, "top3.png"
                ));

            var awardIdx = 0;
            foreach (var awardRanking in tallyResult.AwardRankings)
            {
                var award = awardRanking.Award;
                var winnerCount = awardRanking.GetWinners().Count;
                layer.Add(InkElements.CreateExportArea(
                    $"all_{award.Id}_export", "e0a020", 600, 140 * awardIdx, 480 * winnerCount, 120, $"awards_{award.Id}.png"
                    ));

                awardIdx++;
            }
        }

        private void PopulateExportAreas(TrophiesBuilder builder)
        {
            var layer = builder.Layers["Export"];
            layer.RemoveNodes();

            foreach (var section in builder.TrophySections)
            {
                var exportArea = section.CreateExportArea();
                layer.Add(exportArea);
            }
        }

        private void PopulateBoxes(TrophiesBuilder builder)
        {
            var layer = builder.Layers["Boxes"];
            layer.RemoveNodes();

            foreach (var section in builder.TrophySections)
            {
                foreach (var box in section.CreateBoxes())
                    layer.Add(box);
            }
        }
    }
}
