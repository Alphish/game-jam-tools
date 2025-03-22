using System.Linq;
using System.Xml.Linq;

namespace Alphicsh.JamTally.Trophies.Image
{
    internal static class InkTrophiesExtensions
    {
        public static void SetPlainAttribute(this XElement element, string name, object? value)
            => element.SetAttributeValue(name, value);

        public static void SetMainNamespace(this XElement element, string uri)
            => element.SetAttributeValue("xmlns", uri);

        public static void SetNamespace(this XElement element, string identifier, string uri)
            => element.SetAttributeValue(XNamespace.Xmlns + identifier, uri);

        // ----
        // Data
        // ----

        public static bool HasDataAttribute(this XElement element, string name)
            => element.DataAttribute(name) != null;

        public static XAttribute? DataAttribute(this XElement element, string name)
            => element.Attribute("data-" + name);

        public static void SetDataAttribute(this XElement element, string name, object? value)
            => element.SetAttributeValue("data-" + name, value);

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
