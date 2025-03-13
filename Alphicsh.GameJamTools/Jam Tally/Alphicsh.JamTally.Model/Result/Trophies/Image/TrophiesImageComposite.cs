using System.Collections.Generic;
using System.Xml.Linq;

namespace Alphicsh.JamTally.Model.Result.Trophies.Image
{
    public class TrophiesImageComposite
    {
        public TrophiesImage Image { get; init; } = default!;

        public string Id { get; init; } = default!;
        public int X { get; init; }
        public int Y { get; init; }

        private List<TrophiesImageGuide> GuidesList { get; } = new List<TrophiesImageGuide>();
        public IReadOnlyCollection<TrophiesImageGuide> Guides => GuidesList;

        private Dictionary<string, TrophiesImageGuide> GuidesByRole { get; }
            = new Dictionary<string, TrophiesImageGuide>();

        public TrophiesImageGuide AddNewGuide(TrophiesImageLayer layer, string role, int x, int y, int width, int height)
        {
            var guideElement = InkElements.CreateGuide(X + x, Y + y, width, height, Id, role);
            var guide = new TrophiesImageGuide
            {
                Layer = layer,
                Composite = this,
                Role = role,
                Element = guideElement,
                XRelative = x,
                YRelative = y,
                Width = width,
                Height = height,
            };
            layer.Element.Add(guide.Element);
            return AddAndPassGuide(guide);
        }

        public TrophiesImageGuide LoadExistingGuide(XElement element)
        {
            var layer = Image.FindLayerContainingElement(element);
            var role = element.DataAttribute("guide-role")!.Value;
            var containingGroup = !element.Parent!.IsLayer() ? element.Parent! : null;

            var xAbsolute = int.Parse(element.Attribute("x")!.Value);
            var yAbsolute = int.Parse(element.Attribute("y")!.Value);
            var width = int.Parse(element.Attribute("width")!.Value);
            var height = int.Parse(element.Attribute("height")!.Value);

            var guide = new TrophiesImageGuide
            {
                Layer = layer,
                Composite = this,
                Role = role,
                Element = element,
                Group = containingGroup,
                XRelative = xAbsolute - X,
                YRelative = yAbsolute - Y,
                Width = width,
                Height = height,
            };
            return AddAndPassGuide(guide);
        }

        private TrophiesImageGuide AddAndPassGuide(TrophiesImageGuide guide)
        {
            guide.Element.SetPlainAttribute("style", $"fill:#000000;fill-opacity:0.02;opacity:1;stroke:none");
            GuidesList.Add(guide);
            GuidesByRole.Add(guide.Role, guide);
            return guide;
        }

        public TrophiesImageGuide FindGuideByRole(string role)
            => GuidesByRole[role];

        public void CloneTo(int x, int y)
        {
            foreach (var guide in Guides)
            {
                guide.CloneTo(x, y);
            }
        }
    }
}
