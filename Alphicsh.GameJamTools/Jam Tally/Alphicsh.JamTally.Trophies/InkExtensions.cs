using System.Xml.Linq;

namespace Alphicsh.JamTally.Trophies
{
    internal static class InkExtensions
    {
        public static bool HasInkAttribute(this XElement element, string name)
            => element.InkAttribute(name) != null;

        public static XAttribute? InkAttribute(this XElement element, string name)
            => element.Attribute(InkNames.ForInkscape(name));

        public static void SetInkAttribute(this XElement element, string name, object? value)
            => element.SetAttributeValue(InkNames.ForInkscape(name), value);
    }
}
