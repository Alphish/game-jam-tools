using System.Linq;
using System.Xml.Linq;

namespace Alphicsh.JamTally.Model.Result.Trophies.Image
{
    public class TrophiesImageGuide
    {
        public TrophiesImageLayer Layer { get; init; } = default!;
        public TrophiesImageComposite Composite { get; init; } = default!;
        public string Role { get; init; } = default!;

        public XElement? Group { get; init; }
        public XElement Element { get; init; } = default!;

        public int XRelative { get; init; }
        public int YRelative { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }

        public int XAbsolute => Composite.X + XRelative;
        public int YAbsolute => Composite.Y + YRelative;

        public bool CanClone => Group != null;

        public XElement? CloneTo(int x, int y)
        {
            if (Group == null)
                return null;

            var xshift = x - Composite.X;
            var yshift = y - Composite.Y;

            var clone = InkElements.CloneGroup(Group, xshift, yshift);
            clone.Elements().Single(element => element.IsGuide()).Remove();
            Group.AddAfterSelf(clone);

            return clone;
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
