using System.Linq;
using System.Xml.Linq;
using Alphicsh.JamTally.Model.Result;

namespace Alphicsh.JamTally.Trophies.Image.Generators
{
    public class TrophiesEntriesGenerator
    {
        public TrophiesImage Generate(
            XDocument svgDocument,
            TrophiesImageSettings imageSettings,
            JamTallyNewResult result
            )
        {
            var image = TrophiesImage.LoadStub(svgDocument, imageSettings)
                .WithPendingGuides()
                .WithLayer("medals_back", "Medals Back")
                .WithLayer("medals_inner", "Medals Inner", after: "medals_back")
                .WithLayer("medals_rim", "Medals Rim")
                .WithLayer("medals_sheen", "Medals Sheen")
                .WithLayer("medals_outer", "Medals Outer", after: "medals_sheen")
                .WithLayer("text_stroke", "Text Stroke")
                .WithLayer("text_fill", "Text Fill")
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

            var basicMedal = image.FindMedalSection("basic");
            foreach (var entrySection in image.GetEntrySections())
            {
                var x = entrySection.X;
                var y = entrySection.Y;
                var entry = entrySection.Entry;

                basicMedal.MedalComposite
                    .CloneTo("medal_back", x, y)
                    .CloneTo("medal_rim", x, y)
                    .CloneTo("medal_sheen", x, y);

                basicMedal.TextComposite
                    .CloneTo("jam_label", x, y)
                    .CloneWithText("authors_stroke", x, y, entry.Authors)
                    .CloneWithText("authors_fill", x, y, entry.Authors)
                    .CloneWithText("title_stroke", x, y, entry.Title)
                    .CloneWithText("title_fill", x, y, entry.Title)
                    .CloneWithText("desc_stroke", x, y, "Nth place")
                    .CloneWithText("desc_fill", x, y, "Nth place");
            }

            return image;
        }
    }
}
