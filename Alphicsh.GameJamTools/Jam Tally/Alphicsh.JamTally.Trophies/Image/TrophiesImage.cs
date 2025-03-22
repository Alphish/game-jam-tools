using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Alphicsh.JamTally.Trophies.Image
{
    public class TrophiesImage
    {
        public XDocument Document { get; init; } = default!;
        public XElement Root => Document.Root!;

        public int TrophyWidth { get; init; }
        public int TrophyHeight { get; init; }
        public int ColumnWidth { get; init; }
        public int RowHeight { get; init; }

        public static TrophiesImage CreateBlank(int trophyWidth, int trophyHeight, int columnWidth, int rowHeight)
        {
            return TrophiesImageLoader.CreateBlankImage(trophyWidth, trophyHeight, columnWidth, rowHeight);
        }

        // ------
        // Layers
        // ------

        public IReadOnlyCollection<TrophiesLayer> Layers => LayersList;
        private List<TrophiesLayer> LayersList { get; } = new List<TrophiesLayer>();
        private Dictionary<string, TrophiesLayer> LayersById { get; } = new Dictionary<string, TrophiesLayer>();
        private Dictionary<XElement, TrophiesLayer> LayersByElement { get; } = new Dictionary<XElement, TrophiesLayer>();

        public TrophiesImage WithLayer(string id, string label)
        {
            var layer = TrophiesLayer.Create(this, id, label);
            LayersList.Add(layer);
            LayersById.Add(layer.Id, layer);
            LayersByElement.Add(layer.Element, layer);
            return this;
        }

        public TrophiesLayer FindLayer(string id)
            => LayersById[id];

        public TrophiesLayer FindElementLayer(XElement element)
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

        private Dictionary<string, TrophiesComposite> CompositesById { get; } = new Dictionary<string, TrophiesComposite>();

        public TrophiesComposite CreateComposite(string id, GameTrophyLayout layout)
        {
            var composite = TrophiesComposite.Create(this, id, layout);
            CompositesById.Add(composite.Id, composite);
            return composite;
        }

        public TrophiesComposite FindComposite(string id)
            => CompositesById[id];

        // --------
        // Sections
        // --------

        private Dictionary<string, MedalSection> MedalSectionsById { get; } = new Dictionary<string, MedalSection>();

        public TrophiesImage WithMedalSection(string medalType, int row, int column)
        {
            var medalSection = MedalSection.Create(this, medalType, row, column);
            MedalSectionsById.Add(medalType, medalSection);
            return this;
        }

    }
}
