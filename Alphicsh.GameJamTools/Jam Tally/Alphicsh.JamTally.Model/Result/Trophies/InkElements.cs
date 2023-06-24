using System.Xml.Linq;

namespace Alphicsh.JamTally.Model.Result.Trophies
{
    public static class InkElements
    {
        public static XElement CreateExportArea(string id, string fill, int x, int y, int width, int height, string exportName)
        {
            var exportArea = new XElement(InkNames.ForSvg("rect"));
            exportArea.SetPlainattribute("id", id);
            exportArea.SetPlainattribute("style", $"fill:#{fill};opacity:1;stroke:none");
            exportArea.SetPlainattribute("x", x);
            exportArea.SetPlainattribute("y", y);
            exportArea.SetPlainattribute("width", width);
            exportArea.SetPlainattribute("height", height);
            exportArea.SetInkAttribute("export-filename", exportName);
            exportArea.SetInkAttribute("export-xdpi", 96);
            exportArea.SetInkAttribute("export-ydpi", 96);
            return exportArea;
        }

        public static XElement CreateBox(string fill, int x, int y, int width, int height)
        {
            var box = new XElement(InkNames.ForSvg("rect"));
            box.SetPlainattribute("style", $"fill:#{fill};fill-opacity:0.2;opacity:1;stroke:none");
            box.SetPlainattribute("x", x);
            box.SetPlainattribute("y", y);
            box.SetPlainattribute("width", width);
            box.SetPlainattribute("height", height);
            return box;
        }
    }
}
