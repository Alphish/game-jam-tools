using System.Linq;
using System.Xml.Linq;

namespace Alphicsh.JamTally.Trophies.Image
{
    public class TrophiesLayer
    {
        public TrophiesImage Image { get; init; } = default!;

        public string Id { get; init; } = default!;
        public string Label { get; init; } = default!;
        public XElement Element { get; init; } = default!;

        public static TrophiesLayer FindOrCreate(TrophiesImage image, string id, string label)
        {
            var element = FindLayerElement(image, id);
            if (element == null)
            {
                element = CreateLayerElement(id);
                image.Document.Root!.Add(element);
            }

            element.SetInkAttribute("label", label);
            return new TrophiesLayer
            {
                Image = image,
                Id = id,
                Label = label,
                Element = element,
            };
        }

        private static XElement? FindLayerElement(TrophiesImage image, string id)
        {
            return image.Document.Root!.Elements()
                .FirstOrDefault(element => element.IsLayer() && element.Attribute("id")?.Value == id);
        }

        private static XElement CreateLayerElement(string id)
        {
            var layer = new XElement(InkNames.ForSvg("g"));
            layer.SetPlainAttribute("id", id);
            layer.SetInkAttribute("groupmode", "layer");
            return layer;
        }
    }
}
