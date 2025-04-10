using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Alphicsh.JamTally.Model.Result;
using Alphicsh.JamTally.Trophies.Image.Generators;

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

        public static TrophiesImage LoadStub(XDocument svgDocument, TrophiesImageSettings imageSettings)
        {
            svgDocument.Root!.Attribute(XNamespace.Xmlns + "svg")?.Remove();
            return new TrophiesImage
            {
                Document = svgDocument,
                TrophyWidth = imageSettings.TrophyWidth,
                TrophyHeight = imageSettings.TrophyHeight,
                ColumnWidth = imageSettings.ColumnWidth,
                RowHeight = imageSettings.RowHeight,
            };
        }

        public static TrophiesImage CreateStub(TrophiesImageSettings imageSettings)
        {
            var root = new XElement(XNamespace.Get("http://www.w3.org/2000/svg") + "svg");
            root.SetPlainAttribute("id", "trophies_image");
            root.SetPlainAttribute("width", imageSettings.TrophyWidth);
            root.SetPlainAttribute("height", imageSettings.TrophyHeight);
            root.SetPlainAttribute("viewBox", $"0 0 {imageSettings.TrophyWidth} {imageSettings.TrophyHeight}");
            root.SetPlainAttribute("version", "1.1");
            root.SetMainNamespace("http://www.w3.org/2000/svg");
            root.SetNamespace("inkscape", "http://www.inkscape.org/namespaces/inkscape");
            root.SetNamespace("sodipodi", "http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd");
            root.SetNamespace("xlink", "http://www.w3.org/1999/xlink");

            var document = new XDocument(root);
            return LoadStub(document, imageSettings);
        }

        // ------
        // Saving
        // ------

        public string Format()
        {
            var content = Document.ToString();
            content = Regex.Replace(content, "\\s*<tspan", "<tspan");
            content = Regex.Replace(content, "</tspan>\\s*", "</tspan>");
            return content;
        }

        // ------
        // Layers
        // ------

        public IReadOnlyCollection<TrophiesLayer> Layers => LayersList;
        private List<TrophiesLayer> LayersList { get; } = new List<TrophiesLayer>();
        private Dictionary<string, TrophiesLayer> LayersById { get; } = new Dictionary<string, TrophiesLayer>();
        private Dictionary<XElement, TrophiesLayer> LayersByElement { get; } = new Dictionary<XElement, TrophiesLayer>();

        public TrophiesImage WithLayer(string id, string label, string? before = null)
        {
            var layer = TrophiesLayer.FindOrCreate(this, id, label);
            if (before != null)
                PlaceLayerBefore(layer, before);

            LayersList.Add(layer);
            LayersById.Add(layer.Id, layer);
            LayersByElement.Add(layer.Element, layer);
            return this;
        }

        private void PlaceLayerBefore(TrophiesLayer layer, string before)
        {
            var nextSibling = LayersById[before].Element;
            if (layer.Element.NextNode == nextSibling)
                return;

            layer.Element.Remove();
            nextSibling.AddBeforeSelf(layer.Element);
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

        public TrophiesComposite DefineComposite(string id, GameTrophyLayout layout)
        {
            var composite = TrophiesComposite.Create(this, id, layout);
            CompositesById.Add(composite.Id, composite);
            return composite;
        }

        public TrophiesComposite FindComposite(string id)
            => CompositesById[id];

        private Dictionary<string, XElement> PendingGuideElements { get; } = new Dictionary<string, XElement>();

        public TrophiesImage WithPendingGuides()
        {
            foreach (var box in Document.Descendants(InkNames.ForSvg("rect")))
            {
                if (box.HasDataAttribute("guide-composite") && box.HasDataAttribute("guide-role"))
                {
                    var key = box.DataAttribute("guide-composite")!.Value + ":" + box.DataAttribute("guide-role")!.Value;
                    PendingGuideElements[key] = box;
                }
            }
            return this;
        }

        public XElement? FindPendingGuide(TrophiesComposite composite, string role)
        {
            var key = $"{composite.Id}:{role}";
            return PendingGuideElements.TryGetValue(key, out var element) ? element : null;
        }

        // --------
        // Sections
        // --------

        // Medals

        private Dictionary<string, MedalSection> MedalSectionsById { get; } = new Dictionary<string, MedalSection>();
        public MedalSection FindMedalSection(string id) => MedalSectionsById[id];

        public TrophiesImage WithMedalSection(string medalType, int row, int column)
        {
            var medalSection = MedalSection.Create(this, medalType, row, column);
            MedalSectionsById.Add(medalType, medalSection);
            return this;
        }

        // Entries

        private Dictionary<string, EntrySection> EntrySectionsByCode { get; } = new Dictionary<string, EntrySection>();
        public IEnumerable<EntrySection> GetEntrySections() => EntrySectionsByCode.Values;
        public EntrySection FindEntrySection(JamTallyEntry entry) => EntrySectionsByCode[entry.Code];

        public TrophiesImage AddEntrySection(JamTallyEntry tallyEntry, int row, int column)
        {
            var entrySection = EntrySection.Create(this, tallyEntry, row, column);
            EntrySectionsByCode.Add(tallyEntry.Code, entrySection);
            return this;
        }

        // Trophies

        private List<TrophySection> TrophySections { get; } = new List<TrophySection>();
        public IEnumerable<TrophySection> GetTrophySections() => TrophySections;

        public TrophiesImage AddTrophySection(JamTallyEntry tallyEntry, string trophyType, int row, int column)
        {
            var trophySection = TrophySection.Create(this, tallyEntry, trophyType, row, column);
            TrophySections.Add(trophySection);
            return this;
        }

        // Reviewers

        private List<ReviewerSection> ReviewerSections { get; } = new List<ReviewerSection>();
        public IEnumerable<ReviewerSection> GetReviewerSections() => ReviewerSections;

        public TrophiesImage AddReviewerSection(JamTallyVote tallyVote, int row, int column)
        {
            var reviewerSection = ReviewerSection.Create(this, tallyVote, row, column);
            ReviewerSections.Add(reviewerSection);
            return this;
        }
    }
}
