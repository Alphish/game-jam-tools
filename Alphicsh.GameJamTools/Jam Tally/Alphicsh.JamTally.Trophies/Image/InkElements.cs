using System.Xml.Linq;

namespace Alphicsh.JamTally.Trophies.Image
{
    public static class InkElements
    {
        public static XElement CreateBox(int x, int y, int width, int height, string fill, decimal opacity = 0.2m)
        {
            var box = new XElement(InkNames.ForSvg("rect"));
            box.SetPlainAttribute("style", $"fill:#{fill};fill-opacity:{opacity};opacity:1;stroke:none");
            box.SetPlainAttribute("x", x);
            box.SetPlainAttribute("y", y);
            box.SetPlainAttribute("width", width);
            box.SetPlainAttribute("height", height);
            return box;
        }

        public static XElement CreateExportArea(string id, int x, int y, int width, int height, string fill, string exportName, int dpi)
        {
            var exportArea = CreateBox(x, y, width, height, fill, 1m);
            exportArea.SetPlainAttribute("id", id);
            exportArea.SetInkAttribute("export-filename", exportName);
            exportArea.SetInkAttribute("export-xdpi", dpi);
            exportArea.SetInkAttribute("export-ydpi", dpi);
            return exportArea;
        }

        public static XElement CloneGroup(XElement original, int xshift, int yshift)
        {
            var clone = new XElement(original);
            clone.SetPlainAttribute("transform", $"translate({xshift},{yshift})");
            return clone;
        }
    }
}
