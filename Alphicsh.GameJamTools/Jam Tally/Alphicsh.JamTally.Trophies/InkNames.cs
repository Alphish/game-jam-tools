using System.Xml.Linq;

namespace Alphicsh.JamTally.Trophies
{
    public static class InkNames
    {
        public static XName ForSvg(string localName)
            => XName.Get(localName, "http://www.w3.org/2000/svg");

        public static XName ForInkscape(string localName)
            => XName.Get(localName, "http://www.inkscape.org/namespaces/inkscape");
    }
}
