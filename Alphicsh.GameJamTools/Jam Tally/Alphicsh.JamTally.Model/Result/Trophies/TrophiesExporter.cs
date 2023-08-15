using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTally.Model.Result.Trophies
{
    public class TrophiesExporter
    {
        public event EventHandler<TrophiesExportProgressEvent>? ExportProgress;

        private FilePath SvgPath { get; }
        private FilePath ExportDirectoryPath { get; }

        private XDocument Document { get; }
        private IReadOnlyDictionary<string, XElement> Layers { get; }

        private IReadOnlyCollection<XElement> MassExportElements { get; }
        private IReadOnlyCollection<XElement> IndividualExportElements { get; }
        private int ExportedCount { get; set; }
        private int TotalCount => MassExportElements.Count + IndividualExportElements.Count;

        public TrophiesExporter(FilePath svgPath)
        {
            SvgPath = svgPath;
            ExportDirectoryPath = svgPath.GetParentDirectoryPath().Append(svgPath.GetNameWithoutExtension());

            Document = XDocument.Load(svgPath.Value);
            Layers = ExtractLayers(Document);

            MassExportElements = GetElementsToExport(Layers["MassExport"]);
            IndividualExportElements = GetElementsToExport(Layers["Export"]);
            ExportedCount = 0;
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

        // ---------
        // Exporting
        // ---------

        private static SvgToPngExporter SvgToPngExporter { get; } = new SvgToPngExporter();

        public void Export()
        {
            ReportProgress($"{TotalCount} elements to export found.");

            foreach (var element in MassExportElements)
            {
                ExportMassItem(element);
            }
            foreach (var element in IndividualExportElements)
            {
                ExportIndividualItem(element);
            }

            ReportProgress("Export finished!");
        }

        private void ExportMassItem(XElement element)
        {
            try
            {
                var elementId = element.Attribute("id")!.Value;
                var exportFilename = element.InkAttribute("export-filename")?.Value;
                if (exportFilename == null)
                    throw new ArgumentException($"Cannot find 'inkscape:export-filename' attribute for element '{elementId}'.");

                var path = ExportDirectoryPath.Append(exportFilename);
                SvgToPngExporter.ExportPng(SvgPath, path, elementId, elementId == "top3_export" ? 192 : 96);

                ExportedCount++;
                ReportProgress($"Element '{elementId}' exported!");
            }
            catch (Exception e)
            {
                ReportProgress(e.Message);
            }
        }

        private void ExportIndividualItem(XElement element)
        {
            try
            {
                var elementId = element.Attribute("id")!.Value;
                var exportFilename = element.InkAttribute("export-filename")?.Value;
                if (exportFilename == null)
                    throw new ArgumentException($"Cannot find 'inkscape:export-filename' attribute for element '{elementId}'.");

                var subdirectoryPath = ExportDirectoryPath.Append(Path.GetFileNameWithoutExtension(exportFilename));
                var filenameRoot = elementId.Remove(elementId.Length - "_export".Length);
                filenameRoot = filenameRoot.Substring(elementId.IndexOf('_') + 1);

                var smallPath = subdirectoryPath.Append(filenameRoot + "120.png");
                SvgToPngExporter.ExportPng(SvgPath, smallPath, elementId, 96);

                var largePath = subdirectoryPath.Append(filenameRoot + "240.png");
                SvgToPngExporter.ExportPng(SvgPath, largePath, elementId, 192);

                ExportedCount++;
                ReportProgress($"Element '{elementId}' exported!");
            }
            catch (Exception e)
            {
                ReportProgress(e.Message);
            }
        }

        private void ReportProgress(string message)
        {
            var e = new TrophiesExportProgressEvent { ExportedItems = ExportedCount, TotalItems = TotalCount, Message = message };
            ExportProgress?.Invoke(this, e);
        }
    }
}
