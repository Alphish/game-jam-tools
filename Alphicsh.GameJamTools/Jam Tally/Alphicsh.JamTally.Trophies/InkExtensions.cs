using System.Linq;
using System.Xml.Linq;

namespace Alphicsh.JamTally.Trophies
{
    internal static class InkExtensions
    {
        public static bool HasAttribute(this XElement element, string name)
            => element.Attribute(name) != null;

        public static void SetPlainAttribute(this XElement element, string name, object? value)
            => element.SetAttributeValue(name, value);

        // ----
        // Data
        // ----

        public static bool HasDataAttribute(this XElement element, string name)
            => element.DataAttribute(name) != null;

        public static XAttribute? DataAttribute(this XElement element, string name)
            => element.Attribute("data-" + name);

        public static void SetDataAttribute(this XElement element, string name, object? value)
            => element.SetAttributeValue("data-" + name, value);

        // --------
        // Inkscape
        // --------

        public static bool HasInkAttribute(this XElement element, string name)
            => element.InkAttribute(name) != null;

        public static XAttribute? InkAttribute(this XElement element, string name)
            => element.Attribute(InkNames.ForInkscape(name));

        public static void SetInkAttribute(this XElement element, string name, object? value)
            => element.SetAttributeValue(InkNames.ForInkscape(name), value);

        // -------------
        // Element types
        // -------------

        public static bool IsLayer(this XElement element)
            => element.Name.LocalName == "g" && element.InkAttribute("groupmode")?.Value.ToLowerInvariant() == "layer";

        public static bool IsGuide(this XElement element)
            => element.Name.LocalName == "rect" && element.HasDataAttribute("guide-composite");

        public static bool IsGuideGroup(this XElement element)
            => element.Name.LocalName == "g" && element.ExtractGuide() != null;

        public static XElement? ExtractGuide(this XElement element)
        {
            if (element.Name.LocalName != "g")
                return null;

            return element.Elements().SingleOrDefault(child => child.IsGuide());
        }
    }
}
