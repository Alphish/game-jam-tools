using System.Linq;
using System.Xml.Linq;
using Alphicsh.JamTally.Model.Result.Trophies.Data;

namespace Alphicsh.JamTally.Model.Result.Trophies.Image
{
    public class TrophiesImageLoader
    {
        // ----
        // Core
        // ----

        public static TrophiesImage CreateCoreTemplate(XDocument document)
        {
            var image = new TrophiesImage { Document = document};

            var loader = new TrophiesImageLoader();
            loader.CreateNewLayers(image);
            loader.CreateAllMedalComposites(image);
            loader.CreateAllMedalGuides(image);

            return image;
        }

        private void CreateNewLayers(TrophiesImage image)
        {
            image.CreateNewLayer("mass_export", "Mass Export");
            image.CreateNewLayer("export", "Export");
            image.CreateNewLayer("medals_back", "Medals Back");
            image.CreateNewLayer("medals_inner", "Medals Inner");
            image.CreateNewLayer("medals_rim", "Medals Rim");
            image.CreateNewLayer("medals_sheen", "Medals Sheen");
            image.CreateNewLayer("medals_outer", "Medals Outer");
            image.CreateNewLayer("text_stroke", "Text Stroke");
            image.CreateNewLayer("text_fill", "Text Fill");

            var layersToRemove = image.Root.Elements().Where(element => element.IsLayer()).ToList();
            foreach (var layer in layersToRemove)
            {
                layer.Remove();
            }

            foreach (var layer in image.Layers)
            {
                image.Root.Add(layer.Element);
            }
        }

        private void CreateAllMedalComposites(TrophiesImage image)
        {
            CreateMedalComposites(image, "gold", order: 0);
            CreateMedalComposites(image, "silver", order: 1);
            CreateMedalComposites(image, "bronze", order: 2);
            CreateMedalComposites(image, "award", order: 3);
            CreateMedalComposites(image, "basic", order: 4);
        }

        private void CreateMedalComposites(TrophiesImage image, string name, int order)
        {
            var id = $"medal_{name}";
            var textId = $"medaltext_{name}";
            var x = 0;
            var y = (-5 + order) * 140;

            image.CreateNewComposite(id, x, y);
            image.CreateNewComposite(textId, x, y);
        }


        private void CreateAllMedalGuides(TrophiesImage image)
        {
            CreateMedalGuides(image, "gold");
            CreateMedalGuides(image, "silver");
            CreateMedalGuides(image, "bronze");
            CreateMedalGuides(image, "award");
            CreateMedalGuides(image, "basic");
        }

        private void CreateMedalGuides(TrophiesImage image, string name)
        {
            var composite = image.FindComposite($"medal_{name}");
            composite.AddNewGuide(image.FindLayer("medals_back"), role: "medal_back", 20, 0, 120, 120);
            composite.AddNewGuide(image.FindLayer("medals_rim"), role: "medal_rim", 20, 0, 120, 120);
            composite.AddNewGuide(image.FindLayer("medals_sheen"), role: "medal_sheen", 20, 0, 120, 120);
            composite.AddNewGuide(image.FindLayer("text_fill"), role: "jam_title", 0, 0, 20, 120);

            var textComposite = image.FindComposite($"medaltext_{name}");
            textComposite.AddNewGuide(image.FindLayer("text_stroke"), role: "authors_stroke", 150, 4, 330, 24);
            textComposite.AddNewGuide(image.FindLayer("text_fill"), role: "authors_fill", 150, 4, 330, 24);
            textComposite.AddNewGuide(image.FindLayer("text_stroke"), role: "title_stroke", 150, 32, 330, 56);
            textComposite.AddNewGuide(image.FindLayer("text_fill"), role: "title_fill", 150, 32, 330, 56);
            textComposite.AddNewGuide(image.FindLayer("text_stroke"), role: "desc_stroke", 150, 92, 330, 24);
            textComposite.AddNewGuide(image.FindLayer("text_fill"), role: "desc_fill", 150, 92, 330, 24);
        }

        // -------
        // Entries
        // -------

        public static TrophiesImage CreateEntriesTemplate(XDocument document, TrophiesInput input)
        {
            var image = new TrophiesImage { Document = document };

            var loader = new TrophiesImageLoader();
            loader.LoadExistingLayers(image);
            loader.CreateAllMedalComposites(image);
            loader.CreateAllEntryComposites(image, input);
            loader.LoadExistingGuides(image);
            loader.CreateAllEntryGuides(image, input);

            return image;
        }

        private void CreateAllEntryComposites(TrophiesImage image, TrophiesInput input)
        {
            for (var i = 0; i < input.Entries.Count; i++)
            {
                var entry = input.Entries.ElementAt(i);
                CreateEntryComposites(image, entry, i);
            }
        }

        private void CreateEntryComposites(TrophiesImage image, JamTrophyEntry entry, int order)
        {
            var id = $"entry_{entry.Id}";
            var textId = $"entrytext_{entry.Id}";
            var x = -1200;
            var y = 140 * order;

            image.CreateNewComposite(id, x, y);
            image.CreateNewComposite(textId, x, y);
        }

        public void CreateAllEntryGuides(TrophiesImage image, TrophiesInput input)
        {

            foreach (var entry in input.Entries)
                CreateEntryGuides(image, entry);
        }

        private void CreateEntryGuides(TrophiesImage image, JamTrophyEntry entry)
        {
            var composite = image.FindComposite($"entry_{entry.Id}");
            composite.AddNewGuide(image.FindLayer("medals_inner"), role: "medal_inner", 20, 0, 120, 120);
            composite.AddNewGuide(image.FindLayer("medals_outer"), role: "medal_outer", 0, 0, 520, 120);

            var textComposite = image.FindComposite($"entrytext_{entry.Id}");
            textComposite.AddNewGuide(image.FindLayer("text_stroke"), role: "authors_stroke", 150, 4, 370, 24);
            textComposite.AddNewGuide(image.FindLayer("text_fill"), role: "authors_fill", 150, 4, 370, 24);
            textComposite.AddNewGuide(image.FindLayer("text_stroke"), role: "title_stroke", 150, 32, 370, 56);
            textComposite.AddNewGuide(image.FindLayer("text_fill"), role: "title_fill", 150, 32, 370, 56);
            textComposite.AddNewGuide(image.FindLayer("text_stroke"), role: "desc_stroke", 150, 92, 370, 24);
            textComposite.AddNewGuide(image.FindLayer("text_fill"), role: "desc_fill", 150, 92, 370, 24);
        }

        // --------
        // Trophies
        // --------

        public static TrophiesImage LoadTemplateFrom(XDocument document, TrophiesInput input)
        {
            var image = new TrophiesImage { Document = document };

            var loader = new TrophiesImageLoader();
            loader.LoadExistingLayers(image);
            loader.CreateAllMedalComposites(image);
            loader.CreateAllEntryComposites(image, input);
            loader.LoadExistingGuides(image);

            return image;
        }

        private void LoadExistingLayers(TrophiesImage image)
        {
            foreach (var element in image.Root.Elements())
            {
                if (!element.IsLayer())
                    continue;

                image.LoadExistingLayer(element);
            }
        }

        public void LoadExistingGuides(TrophiesImage image)
        {
            foreach (var element in image.Root.Descendants())
            {
                if (!element.IsGuide())
                    continue;

                var compositeId = element.DataAttribute("guide-composite")!.Value;
                var composite = image.FindComposite(compositeId);
                composite.LoadExistingGuide(element);
            }
        }
    }
}
