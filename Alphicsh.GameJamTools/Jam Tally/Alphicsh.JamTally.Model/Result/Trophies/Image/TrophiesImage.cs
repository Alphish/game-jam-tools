using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Alphicsh.JamTally.Model.Result.Trophies.Image
{
    public class TrophiesImage
    {
        public XDocument Document { get; init; } = default!;
        public XElement Root => Document.Root!;

        // ------
        // Layers
        // ------

        public IReadOnlyCollection<TrophiesImageLayer> Layers => LayersList;
        private List<TrophiesImageLayer> LayersList { get; }
            = new List<TrophiesImageLayer>();

        private Dictionary<string, TrophiesImageLayer> LayersById { get; }
            = new Dictionary<string, TrophiesImageLayer>();
        
        private Dictionary<XElement, TrophiesImageLayer> LayersByElement { get; }
            = new Dictionary<XElement, TrophiesImageLayer>();

        public TrophiesImageLayer CreateNewLayer(string id, string label)
        {
            var layer = new TrophiesImageLayer
            {
                Image = this,
                Id = id,
                Label = label,
                Element = InkElements.CreateLayer(id, label),
            };
            return AddAndPassLayer(layer);
        }

        public TrophiesImageLayer LoadExistingLayer(XElement element)
        {
            if (!element.IsLayer())
                throw new InvalidOperationException("Trying to create a layer from non-layer element.");

            var id = element.Attribute("id")!.Value;
            var label = element.InkAttribute("label")!.Value;

            var layer = new TrophiesImageLayer
            {
                Image = this,
                Id = id,
                Label = label,
                Element = element,
            };
            return AddAndPassLayer(layer);
        }

        private TrophiesImageLayer AddAndPassLayer(TrophiesImageLayer layer)
        {
            LayersList.Add(layer);
            LayersById.Add(layer.Id, layer);
            LayersByElement.Add(layer.Element, layer);
            return layer;
        }

        public TrophiesImageLayer FindLayer(string id)
            => LayersById[id];

        public TrophiesImageLayer FindLayerContainingElement(XElement element)
        {
            var ancestor = element.Parent;
            while (ancestor != null)
            {
                if (LayersByElement.TryGetValue(ancestor, out var layer))
                    return layer;

                ancestor = ancestor.Parent;
            }
            throw new InvalidOperationException($"Could not find layer for the element.");
        }

        // ----------
        // Composites
        // ----------

        private List<TrophiesImageComposite> Composites { get; }
            = new List<TrophiesImageComposite>();

        private Dictionary<string, TrophiesImageComposite> CompositesById { get; }
            = new Dictionary<string, TrophiesImageComposite>();

        public TrophiesImageComposite CreateNewComposite(string id, int x, int y)
        {
            var composite = new TrophiesImageComposite
            {
                Image = this,
                Id = id,
                X = x,
                Y = y,
            };
            return AddAndPassComposite(composite);
        }

        private TrophiesImageComposite AddAndPassComposite(TrophiesImageComposite composite)
        {
            Composites.Add(composite);
            CompositesById.Add(composite.Id, composite);
            return composite;
        }

        public TrophiesImageComposite FindComposite(string id)
            => CompositesById[id];

        public string FindTextColor(string compositeId, string role, bool findStroke)
        {
            var composite = CompositesById[compositeId];
            var guide = composite.FindGuideByRole(role);
            var guideText = guide.Group!.Elements().Single(element => element.Name.LocalName == "text");
            
            var property = findStroke ? "stroke" : "fill";
            var pattern = new Regex(property + "\\:(#\\w{6})");
            var styleText = guideText.Attribute("style")!.Value;
            var match = pattern.Match(styleText);
            if (!match.Success)
                throw new Exception($"Could not find a valid text color for '{compositeId}':'{role}'");

            return match.Groups[1].Value;
        }
    }
}
