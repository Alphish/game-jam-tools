using System.Xml.Linq;

namespace Alphicsh.JamTally.Model.Result.Trophies
{
    internal static class InkExtensions
    {
        public static void SetInkAttribute(this XElement element, string name, object? value)
            => element.SetAttributeValue(InkNames.ForInkscape(name), value);

        public static XAttribute? InkAttribute(this XElement element, string name)
            => element.Attribute(InkNames.ForInkscape(name));

        public static void SetPlainattribute(this XElement element, string name, object? value)
            => element.SetAttributeValue(name, value);
    }
}
