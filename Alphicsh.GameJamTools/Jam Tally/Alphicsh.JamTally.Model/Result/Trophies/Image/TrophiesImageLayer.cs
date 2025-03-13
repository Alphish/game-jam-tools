using System;
using System.Xml.Linq;

namespace Alphicsh.JamTally.Model.Result.Trophies.Image
{
    public class TrophiesImageLayer
    {
        public TrophiesImage Image { get; init; } = default!;

        public string Id { get; init; } = default!;
        public string Label { get; init; } = default!;
        public XElement Element { get; init; } = default!;

        public static TrophiesImageLayer CreateNew(string id, string label)
        {
            return new TrophiesImageLayer
            {
                Id = id,
                Label = label,
                Element = InkElements.CreateLayer(id, label),
            };
        }

        public static TrophiesImageLayer LoadExisting(XElement layer)
        {
            if (!layer.IsLayer())
                throw new InvalidOperationException("Trying to create a layer from non-layer element.");

            var id = layer.Attribute("id")!.Value;
            var label = layer.InkAttribute("label")!.Value;
            return new TrophiesImageLayer
            {
                Id = id,
                Label = label,
                Element = layer 
            };
        }

        public void CreateBox(string fill, int x, int y, int width, int height)
        {
            var box = InkElements.CreateBox(fill, x, y, width, height);
            Element.Add(box);
        }
    }
}
