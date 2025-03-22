using System.Xml.Linq;

namespace Alphicsh.JamTally.Trophies.Image
{
    internal class TrophiesImageLoader
    {
        public static TrophiesImage CreateBlankImage(int trophyWidth, int trophyHeight, int columnWidth, int rowHeight)
        {
            var root = new XElement(XNamespace.Get("http://www.w3.org/2000/svg") + "svg");
            root.SetPlainAttribute("id", "trophies_image");
            root.SetPlainAttribute("width", trophyWidth);
            root.SetPlainAttribute("height", trophyHeight);
            root.SetPlainAttribute("viewBox", $"0 0 {trophyWidth} {trophyHeight}");
            root.SetPlainAttribute("version", "1.1");
            root.SetMainNamespace("http://www.w3.org/2000/svg");
            root.SetNamespace("inkscape", "http://www.inkscape.org/namespaces/inkscape");
            root.SetNamespace("sodipodi", "http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd");
            root.SetNamespace("xlink", "http://www.w3.org/1999/xlink");

            var document = new XDocument(root);

            return new TrophiesImage
            {
                Document = document,
                TrophyWidth = trophyWidth,
                TrophyHeight = trophyHeight,
                ColumnWidth = columnWidth,
                RowHeight = rowHeight
            };
        }
    }
}
