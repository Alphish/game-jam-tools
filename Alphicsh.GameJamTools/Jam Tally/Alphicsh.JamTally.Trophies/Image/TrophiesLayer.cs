using System.Xml.Linq;

namespace Alphicsh.JamTally.Trophies.Image
{
    public class TrophiesLayer
    {
        public TrophiesImage Image { get; init; } = default!;

        public string Id { get; init; } = default!;
        public string Label { get; init; } = default!;
        public XElement Element { get; init; } = default!;

        public static TrophiesLayer Create(TrophiesImage image, string id, string label)
        {
            var element = InkElements.CreateLayer(id, label);
            image.Document.Root!.Add(element);

            return new TrophiesLayer
            {
                Image = image,
                Id = id,
                Label = label,
                Element = element,
            };
        }
    }
}
