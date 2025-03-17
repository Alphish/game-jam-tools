using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTally.Trophies.Export
{
    internal class TrophiesExportDocument
    {
        private XDocument Document { get; }
        private IReadOnlyDictionary<string, XElement> Layers { get; }

        public FilePath DocumentPath { get; }
        public IReadOnlyCollection<XElement> MassExportElements { get; }
        public IReadOnlyCollection<XElement> IndividualExportElements { get; }

        public TrophiesExportDocument(XDocument document, FilePath documentPath)
        {
            Document = document;
            Layers = ExtractLayers(document);

            DocumentPath = documentPath;
            MassExportElements = GetElementsToExport(Layers["Mass Export"]);
            IndividualExportElements = GetElementsToExport(Layers["Export"]);
        }

        private IReadOnlyDictionary<string, XElement> ExtractLayers(XDocument document)
        {
            return document.Root!.Elements()
                .Where(element => element.Name.LocalName == "g")
                .ToDictionary(element => element.InkAttribute("label")!.Value, element => element, StringComparer.OrdinalIgnoreCase);
        }

        private IReadOnlyCollection<XElement> GetElementsToExport(XElement layer)
        {
            return layer.Elements()
                .Where(element => element.Attribute("id")?.Value.EndsWith("_export") ?? false)
                .ToList();
        }
    }
}
