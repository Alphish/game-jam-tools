using System.Drawing;
using System.Xml.Linq;

namespace Alphicsh.JamTally.Trophies.Image
{
    public class TrophiesGuide
    {
        public TrophiesLayer Layer { get; init; } = default!;
        public TrophiesComposite Composite { get; init; } = default!;
        public string Role { get; init; } = default!;

        public XElement Element { get; init; } = default!;

        public int X { get; init; }
        public int Y { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }

        public static TrophiesGuide Create(TrophiesLayer layer, TrophiesComposite composite, string role, Rectangle area, string fill, decimal opacity = 0.1m)
        {
            var element = InkElements.CreateBox(area.X, area.Y, area.Width, area.Height, fill, opacity);
            element.SetDataAttribute("guide-composite", composite);
            element.SetDataAttribute("guide-role", role);

            layer.Element.Add(element);

            return new TrophiesGuide
            {
                Layer = layer,
                Composite = composite,
                Role = role,
                Element = element,
                X = area.X,
                Y = area.Y,
                Width = area.Width,
                Height = area.Height,
            };
        }
    }
}
