using System.Linq;
using System.Xml.Linq;
using Alphicsh.JamTally.Model.Result;

namespace Alphicsh.JamTally.Trophies.Image.Generators
{
    public class TrophiesAssembler
    {
        public TrophiesImage AssembleTrophies(
            XDocument svgDocument,
            TrophiesImageSettings imageSettings,
            JamTallyNewResult result
            )
        {
            var image = TrophiesImage.LoadStub(svgDocument, imageSettings)
                .WithPendingGuides()
                .WithLayer("medals_back", "Medals Back")
                .WithLayer("medals_rim", "Medals Rim")
                .WithLayer("medals_sheen", "Medals Sheen")
                .WithLayer("text_stroke", "Text Stroke")
                .WithLayer("text_fill", "Text Fill")
                .WithLayer("medals_inner", "Medals Inner", before: "medals_rim")
                .WithLayer("medals_outer", "Medals Outer", before: "text_stroke")
                .WithLayer("export", "Export", before: "medals_back")
                .WithLayer("mass_export", "Mass Export", before: "export")
                .WithMedalSection("gold", row: -5, column: 0)
                .WithMedalSection("silver", row: -4, column: 0)
                .WithMedalSection("bronze", row: -3, column: 0)
                .WithMedalSection("award", row: -2, column: 0)
                .WithMedalSection("basic", row: -1, column: 0);

            var entries = result.Ranking.OrderBy(entry => $"{entry.Title} by {entry.Authors}").ToList();
            for (var i = 0; i < entries.Count; i++)
            {
                image.AddEntrySection(entries[i], i, -2);
            }

            var ranking = result.Ranking.ToList();
            for (var i = 0; i < ranking.Count; i++)
            {
                var entry = ranking[i];

                image.AddTrophySection(entry, "rank", i, 0);
                if (!entry.Awards.Any())
                    continue;

                image.AddTrophySection(entry, "all", i, -1);
                for (var j = 0; j < entry.Awards.Count; j++)
                {
                    image.AddTrophySection(entry, entry.Awards[j].Id, i, j + 1);
                }
            }

            var reviewersStartingRow = ranking.Count + 1;
            var bestReviewers = result.Votes
                .Where(vote => result.TopReviewers.Contains(vote.Voter))
                .OrderBy(vote => vote.Voter)
                .ToList();
            for (var i = 0; i < bestReviewers.Count; i++)
            {
                image.AddReviewerSection(bestReviewers[i], reviewersStartingRow + i, 0);
            }

            // putting the trophies together

            var exportLayer = image.FindLayer("export");
            foreach (var trophySection in image.GetTrophySections())
            {
                var x = trophySection.X;
                var y = trophySection.Y;

                var entrySection = image.FindEntrySection(trophySection.Entry);
                var medalSection = image.FindMedalSection(trophySection.MedalType);

                entrySection.MedalComposite
                    .CloneTo("medal_inner", x, y)
                    .CloneTo("medal_outer", x, y);

                var strokeColor = medalSection.TextComposite.GetGuide("title_stroke").GetStroke();
                var fillColor = medalSection.TextComposite.GetGuide("title_fill").GetFill();

                entrySection.TextComposite
                    .CloneTo("authors_stroke", x, y, stroke: strokeColor)
                    .CloneTo("authors_fill", x, y, fill: fillColor)
                    .CloneTo("title_stroke", x, y, stroke: strokeColor)
                    .CloneTo("title_fill", x, y, fill: fillColor)
                    .CloneWithText("desc_stroke", x, y, trophySection.Description, stroke: strokeColor)
                    .CloneWithText("desc_fill", x, y, trophySection.Description, fill: fillColor);

                medalSection.MedalComposite
                    .CloneTo("medal_back", x, y)
                    .CloneTo("medal_rim", x, y)
                    .CloneTo("medal_sheen", x, y);

                medalSection.TextComposite.CloneTo("jam_label", x, y);

                var exportArea = InkElements.CreateExportArea(
                    trophySection.ExportId,
                    trophySection.X, trophySection.Y, image.TrophyWidth, image.TrophyHeight,
                    "242424", trophySection.Entry.EntryId + ".png", 96);

                exportLayer.Element.Add(exportArea);
            }

            // best reviewer

            foreach (var reviewerSection in image.GetReviewerSections())
            {
                var reviewerX = reviewerSection.X;
                var reviewerY = reviewerSection.Y;
                var awardMedal = image.FindMedalSection("award");
                awardMedal.MedalComposite
                    .CloneTo("medal_back", reviewerX, reviewerY)
                    .CloneTo("medal_rim", reviewerX, reviewerY)
                    .CloneTo("medal_sheen", reviewerX, reviewerY);

                awardMedal.TextComposite
                    .CloneTo("jam_label", reviewerX, reviewerY)
                    .CloneWithText("title_stroke", reviewerX, reviewerY - 15, reviewerSection.Voter)
                    .CloneWithText("title_fill", reviewerX, reviewerY - 15, reviewerSection.Voter)
                    .CloneWithText("desc_stroke", reviewerX, reviewerY - 15, "Best Reviewer")
                    .CloneWithText("desc_fill", reviewerX, reviewerY - 15, "Best Reviewer");

                var reviewerExport = InkElements.CreateExportArea(reviewerSection.ExportId, reviewerX, reviewerY, image.TrophyWidth, image.TrophyHeight, "242424", $"Best Reviewer - {reviewerSection.Voter}.png", 96);
                exportLayer.Element.Add(reviewerExport);
            }

            // mass export

            var massExportLayer = image.FindLayer("mass_export");
            var rankingArea = InkElements.CreateExportArea(
                "ranking_export", 0, 0, image.TrophyWidth, image.RowHeight * (result.Ranking.Count - 1) + image.TrophyHeight, "224466", "ranking.png", 96
                );
            massExportLayer.Element.Add(rankingArea);

            var top3Area = InkElements.CreateExportArea(
                "top3_export", 0, 0, image.TrophyWidth, image.RowHeight * 2 + image.TrophyHeight, "664422", "top3.png", 192
                );
            massExportLayer.Element.Add(top3Area);

            return image;
        }
    }
}
