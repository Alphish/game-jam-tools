using System.Xml.Linq;

namespace Alphicsh.JamTally.Model.Result.Trophies
{
    public static class InkElements
    {
        public static XElement CreateLayer(string id, string label)
        {
            var layer = new XElement(InkNames.ForSvg("g"));
            layer.SetPlainAttribute("id", id);
            layer.SetInkAttribute("groupmode", "layer");
            layer.SetInkAttribute("label", label);
            return layer;
        }

        public static XElement CreateExportArea(string id, string fill, int x, int y, int width, int height, string exportName)
        {
            var exportArea = new XElement(InkNames.ForSvg("rect"));
            exportArea.SetPlainAttribute("id", id);
            exportArea.SetPlainAttribute("style", $"fill:#{fill};opacity:1;stroke:none");
            exportArea.SetPlainAttribute("x", x);
            exportArea.SetPlainAttribute("y", y);
            exportArea.SetPlainAttribute("width", width);
            exportArea.SetPlainAttribute("height", height);
            exportArea.SetInkAttribute("export-filename", exportName);
            exportArea.SetInkAttribute("export-xdpi", 96);
            exportArea.SetInkAttribute("export-ydpi", 96);
            return exportArea;
        }

        public static XElement CreateBox(string fill, int x, int y, int width, int height)
        {
            var box = new XElement(InkNames.ForSvg("rect"));
            box.SetPlainAttribute("style", $"fill:#{fill};fill-opacity:0.2;opacity:1;stroke:none");
            box.SetPlainAttribute("x", x);
            box.SetPlainAttribute("y", y);
            box.SetPlainAttribute("width", width);
            box.SetPlainAttribute("height", height);
            return box;
        }

        public static XElement CreateGuide(int x, int y, int width, int height, string composite, string role)
        {
            var guide = CreateBox("000000", x, y, width, height);
            guide.SetPlainAttribute("style", $"fill:#000000;fill-opacity:0.02;opacity:1;stroke:none");
            guide.SetDataAttribute("guide-composite", composite);
            guide.SetDataAttribute("guide-role", role);
            return guide;
        }
        
        public static XElement CloneGroup(XElement original, int xshift, int yshift)
        {
            var clone = new XElement(original);
            clone.SetPlainAttribute("transform", $"translate({xshift},{yshift})");
            return clone;
        }
    }
}
