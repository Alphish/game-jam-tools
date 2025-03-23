using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Alphicsh.JamTally.Model.Result.Trophies.Data;
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

            var content = image.Document.ToString();
            content = Regex.Replace(content, "\\s*<tspan", "<tspan");
            content = Regex.Replace(content, "</tspan>\\s*", "</tspan>");
            File.WriteAllText(destinationPath.Value, content);
        }

        // -------
        // Entries
        // -------

        public void GenerateEntriesTemplate(FilePath sourcePath, FilePath destinationPath)
        {
            EnsureDifferentSavePath(sourcePath, destinationPath);
            var document = LoadImageDocument(sourcePath);
            var input = TrophiesInput.Parse();

            var image = TrophiesImageLoader.CreateEntriesTemplate(document, input);
            foreach (var entry in input.Entries)
                AddEntryBase(image, entry);

            SaveImage(image, destinationPath);
        }

        private void AddEntryBase(TrophiesImage image, JamTrophyEntry entry)
        {
            var entryComposite = image.FindComposite($"entry_{entry.Id}");
            var cloneX = entryComposite.X;
            var cloneY = entryComposite.Y;

            var medalComposite = image.FindComposite("medal_basic");
            medalComposite.CloneTo(cloneX, cloneY);

            var textComposite = image.FindComposite("medaltext_basic");
            textComposite.FindGuideByRole("authors_stroke").CloneWithText(cloneX, cloneY, entry.Authors);
            textComposite.FindGuideByRole("authors_fill").CloneWithText(cloneX, cloneY, entry.Authors);
            textComposite.FindGuideByRole("title_stroke").CloneWithText(cloneX, cloneY, entry.Title);
            textComposite.FindGuideByRole("title_fill").CloneWithText(cloneX, cloneY, entry.Title);
            textComposite.FindGuideByRole("desc_stroke").CloneWithText(cloneX, cloneY, "Xth place");
            textComposite.FindGuideByRole("desc_fill").CloneWithText(cloneX, cloneY, "Xth place");
        }

        // --------
        // Trophies
        // --------

        public void CompileTrophies(JamTallyResult tallyResult, FilePath sourcePath, FilePath destinationPath)
        {
            EnsureDifferentSavePath(sourcePath, destinationPath);

            var document = LoadImageDocument(sourcePath);
            var trophiesData = JamTrophiesData.Compile(tallyResult);
            var image = TrophiesImageLoader.LoadTemplateFrom(document, trophiesData.Input);

            foreach (var trophy in trophiesData.Trophies)
            {
                ProcessTrophy(image, trophy);
            }

            GenerateExportBoxes(image, trophiesData);
            GenerateBestReviewerStub(image, trophiesData.Input);

            SaveImage(image, destinationPath);
        }

        private void ProcessTrophy(TrophiesImage image, JamTrophy trophy)
        {
            var entryComposite = image.FindComposite($"entry_{trophy.Entry.Id}");
            entryComposite.CloneTo(trophy.X, trophy.Y);

            var medalComposite = image.FindComposite($"medal_{trophy.Medal}");
            medalComposite.CloneTo(trophy.X, trophy.Y);

            var entryTextComposite = image.FindComposite($"entrytext_{trophy.Entry.Id}");
            var medalTextComposite = image.FindComposite($"medaltext_{trophy.Medal}");
            var fillColor = image.FindTextColor($"medaltext_{trophy.Medal}", "title_fill", findStroke: false);
            var strokeColor = image.FindTextColor($"medaltext_{trophy.Medal}", "title_stroke", findStroke: true);

            CloneEntryTextComponent("authors_stroke", isStroke: true);
            CloneEntryTextComponent("authors_fill", isStroke: false);
            CloneEntryTextComponent("title_stroke", isStroke: true);
            CloneEntryTextComponent("title_fill", isStroke: false);
            CloneMedalTextComponent("desc_stroke", trophy.Description);
            CloneMedalTextComponent("desc_fill", trophy.Description);

            void CloneEntryTextComponent(string role, bool isStroke)
            {
                var entryGuide = entryTextComposite.FindGuideByRole(role);
                if (!entryGuide.CanClone)
                    return;

                var clone = entryGuide.CloneTo(trophy.X, trophy.Y)!;
                var textElements = clone.Descendants().Where(element => element.Name.LocalName == "text");

                var property = isStroke ? "stroke" : "fill";
                var pattern = new Regex(property + "\\:(#\\w{6})");
                var replacement = property + ":" + (isStroke ? strokeColor : fillColor);
                foreach (var textElement in textElements)
                {
                    var styleText = textElement.Attribute("style")!.Value;
                    var newStyleText = pattern.Replace(styleText, replacement);
                    textElement.SetAttributeValue("style", newStyleText);
                }
            }

            void CloneMedalTextComponent(string role, string text)
            {
                var medalGuide = medalTextComposite.FindGuideByRole(role);
                if (!medalGuide.CanClone)
                    return;

                medalGuide.CloneWithText(trophy.X, trophy.Y, text);
            }
        }

        private void GenerateExportBoxes(TrophiesImage image, JamTrophiesData trophiesData)
        {
            // per-trophy exports
            var exportLayer = image.FindLayer("export");
            exportLayer.Element.RemoveNodes();

            foreach (var trophy in trophiesData.Trophies)
            {
                var area = InkElements.CreateExportArea(trophy.Entry.Id + "_" + trophy.Qualifier + "_export", "363636", trophy.X, trophy.Y, 520, 120, trophy.Entry.Entry.Id + ".png");
                exportLayer.Element.Add(area);
            }

            // mass exports
            var massExportLayer = image.FindLayer("mass_export");
            massExportLayer.Element.RemoveNodes();

            var allExport = InkElements.CreateExportArea("top3_export", "004080", 0, 0, 520, 140 * trophiesData.EntriesCount - 20, "ranking.png");
            massExportLayer.Element.Add(allExport);
            var top3Export = InkElements.CreateExportArea("top3_export", "ff8000", 0, 0, 520, 400, "top3.png");
            massExportLayer.Element.Add(top3Export);
        }

        private void GenerateBestReviewerStub(TrophiesImage image, TrophiesInput input)
        {
            var trophyX = 0;
            var trophyY = 140 * (input.Entries.Count + 1);

            var layer = image.FindLayer("export");
            var area = InkElements.CreateExportArea("best_reviewer_export", "363636", trophyX, trophyY, 520, 120, "Best Reviewer.png");
            layer.Element.Add(area);

            var medalComposite = image.FindComposite("medal_award");
            medalComposite.CloneTo(trophyX, trophyY);

            var medalTextComposite = image.FindComposite($"medaltext_award");
            CloneMedalTextComponent("title_stroke", "Username");
            CloneMedalTextComponent("title_fill", "Username");
            CloneMedalTextComponent("desc_stroke", "Best Reviewer");
            CloneMedalTextComponent("desc_fill", "Best Reviewer");

            void CloneMedalTextComponent(string role, string text)
            {
                var medalGuide = medalTextComposite.FindGuideByRole(role);
                if (!medalGuide.CanClone)
                    return;

                medalGuide.CloneWithText(trophyX, trophyY - 14, text);
            }
        }

        // -------
        // Helpers
        // -------

        private void EnsureDifferentSavePath(FilePath sourcePath, FilePath destinationPath)
        {
            if (sourcePath == destinationPath)
                throw new InvalidOperationException("This isn't very tested, so let's not overwrite things just yet.");
        }

        private XDocument LoadImageDocument(FilePath sourcePath)
        {
            var document = XDocument.Load(sourcePath.Value);
            document.Root!.Attributes(XName.Get("svg", @"http://www.w3.org/2000/xmlns/")).Remove();
            return document;
        }

        private void SaveImage(TrophiesImage image, FilePath destinationPath)
        {
            var content = image.Document.ToString();
            content = Regex.Replace(content, "\\s*<tspan", "<tspan");
            content = Regex.Replace(content, "</tspan>\\s*", "</tspan>");
            File.WriteAllText(destinationPath.Value, content);
        }
    }
}
