using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
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

        // -----
        // Style
        // -----

        private static Regex FillRegex { get; } = new Regex(@"fill\:#(\w{6})");
        private static Regex StrokeRegex { get; } = new Regex(@"stroke\:#(\w{6})");

        public string GetFill() => GetStyle(FillRegex);
        public string GetStroke() => GetStyle(StrokeRegex);

        private string GetStyle(Regex searchPattern)
        {
            var textElement = Group!.Elements().Single(element => element != Element);
            var styleText = textElement.Attribute("style")!.Value;
            var match = searchPattern.Match(styleText);
            if (!match.Success)
                throw new Exception($"Could not find a valid text color for '{Composite.Id}':'{Role}'");

            return match.Groups[1].Value;
        }

        public void CloneTo(int x, int y, string? fill = null, string? stroke = null)
        {
            if (Group == null)
                return;

            var xshift = x - Composite.X;
            var yshift = y - Composite.Y;

            var clone = InkElements.CloneGroup(Group, xshift, yshift);
            clone.Elements().Single(element => element.IsGuide()).Remove();
            TryReplaceFillAndStroke(clone.Descendants(InkNames.ForSvg("text")), fill, stroke);

            Group.AddAfterSelf(clone);
        }

        public void CloneWithText(int x, int y, string text, string? fill = null, string? stroke = null)
        {
            if (Group == null)
                return;

            var xshift = x - Composite.X;
            var yshift = y - Composite.Y;

            var clone = InkElements.CloneGroup(Group, xshift, yshift);
            clone.Elements().Single(element => element.IsGuide()).Remove();
            clone.Descendants().Single(element => element.Name.LocalName == "tspan").Value = text;
            TryReplaceFillAndStroke(clone.Descendants(InkNames.ForSvg("text")), fill, stroke);

            Group.AddAfterSelf(clone);
        }

        private void TryReplaceFillAndStroke(IEnumerable<XElement> elements, string? fill = null, string? stroke = null)
        {
            if (fill != null)
            {
                foreach (var element in elements)
                    element.ReplaceStyle(FillRegex, "fill:#" + fill);
            }

            if (stroke != null)
            {
                foreach (var element in elements)
                    element.ReplaceStyle(StrokeRegex, "stroke:#" + stroke);
            }
        }
    }
}
