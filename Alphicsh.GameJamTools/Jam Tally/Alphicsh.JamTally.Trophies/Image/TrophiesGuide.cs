using System.Drawing;
using System.Linq;
using System.Xml.Linq;

namespace Alphicsh.JamTally.Trophies.Image
{
    public class TrophiesGuide
    {
        public TrophiesLayer Layer { get; init; } = default!;
        public TrophiesComposite Composite { get; init; } = default!;
        public string Role { get; init; } = default!;

        public XElement Element { get; init; } = default!;
        public XElement? Group => !Element.Parent!.IsLayer() ? Element.Parent : null;

        public int X { get; init; }
        public int Y { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }

        public static TrophiesGuide FindOrCreate(TrophiesLayer layer, TrophiesComposite composite, string role, Rectangle area, string fill, decimal opacity = 0.1m)
        {
            var element = layer.Image.FindPendingGuide(composite, role);
            if (element == null)
            {
                element = InkElements.CreateBox(area.X, area.Y, area.Width, area.Height, fill, opacity);
                element.SetDataAttribute("guide-composite", composite.Id);
                element.SetDataAttribute("guide-role", role);
                layer.Element.Add(element);
            }

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

        public void CloneTo(int x, int y)
        {
            if (Group == null)
                return;

            var xshift = x - Composite.X;
            var yshift = y - Composite.Y;

            var clone = InkElements.CloneGroup(Group, xshift, yshift);
            clone.Elements().Single(element => element.IsGuide()).Remove();
            Group.AddAfterSelf(clone);
        }

        public void CloneWithText(int x, int y, string text)
        {
            if (Group == null)
                return;

            var xshift = x - Composite.X;
            var yshift = y - Composite.Y;

            var clone = InkElements.CloneGroup(Group, xshift, yshift);
            clone.Elements().Single(element => element.IsGuide()).Remove();
            clone.Descendants().Single(element => element.Name.LocalName == "tspan").Value = text;
            Group.AddAfterSelf(clone);
        }
    }
}
