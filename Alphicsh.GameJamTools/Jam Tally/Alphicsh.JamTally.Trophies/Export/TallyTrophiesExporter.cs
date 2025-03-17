using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTally.Trophies.Export
{
    public class TallyTrophiesExporter
    {
        private static SvgToPngExporter SvgToPngExporter { get; } = new SvgToPngExporter();

        public async Task Export(FilePath svgPath, FilePath exportDirectory, IProgress<TrophiesExportProgress> progressReporter)
        {
            progressReporter.Report(new TrophiesExportProgress { ExportedItems = 0, TotalItems = 1 });
            var trophiesDocument = LoadTrophiesDocument(svgPath, progressReporter);
            if (trophiesDocument == null)
                return;

            await Task.Run(() => ExportTrophiesDocument(trophiesDocument, exportDirectory, progressReporter));
        }

        private TrophiesExportDocument? LoadTrophiesDocument(FilePath svgPath, IProgress<TrophiesExportProgress> progressReporter)
        {
            XDocument? document = null;
            try
            {
                document = XDocument.Load(svgPath.Value);
            }
            catch (Exception ex)
            {
                progressReporter.Report(new TrophiesExportProgress { ExportedItems = 0, TotalItems = 1, Message = $"An error occurred while reading the file: " + ex.Message });
                return null;
            }

            if (document == null)
            {
                progressReporter.Report(new TrophiesExportProgress { ExportedItems = 0, TotalItems = 1, Message = $"Could not load the trophies images document." });
                return null;
            }

            return new TrophiesExportDocument(document, svgPath);
        }

        private void ExportTrophiesDocument(TrophiesExportDocument document, FilePath exportDirectory, IProgress<TrophiesExportProgress> progressReporter)
        {
            var totalItems = document.MassExportElements.Count + document.IndividualExportElements.Count;
            var exportedItems = 0;
            progressReporter.Report(new TrophiesExportProgress { ExportedItems = exportedItems, TotalItems = totalItems, Message = $"Found {totalItems} elements to export." });

            foreach (var element in document.MassExportElements)
            {
                var elementId = element.Attribute("id")!.Value;
                var message = ExportMassItem(document.DocumentPath, element, exportDirectory);
                exportedItems += 1;
                progressReporter.Report(new TrophiesExportProgress { ExportedItems = exportedItems, TotalItems = totalItems, Message = message });
            }
            foreach (var element in document.IndividualExportElements)
            {
                var message = ExportIndividualItem(document.DocumentPath, element, exportDirectory);
                exportedItems += 1;
                progressReporter.Report(new TrophiesExportProgress { ExportedItems = exportedItems, TotalItems = totalItems, Message = message });
            }

            progressReporter.Report(new TrophiesExportProgress { ExportedItems = exportedItems, TotalItems = totalItems, Message = $"All finished!" });
        }

        private string ExportMassItem(FilePath documentPath, XElement element, FilePath exportDirectory)
        {
            var idAttribute = element.Attribute("id");
            if (idAttribute == null)
                return $"Could not export mass element: Unknown element id.";

            var elementId = idAttribute.Value;
            try
            {
                var exportFilename = element.InkAttribute("export-filename")?.Value;
                if (exportFilename == null)
                    throw new ArgumentException($"Cannot find 'inkscape:export-filename' attribute for element '{elementId}'.");

                var subdirectoryPath = exportDirectory.Append("Overall");

                var exportPath = subdirectoryPath.Append(exportFilename);
                SvgToPngExporter.ExportPng(documentPath, exportPath, elementId, elementId == "top3_export" ? 192 : 96);
                return $"Element '{elementId}' exported!";
            }
            catch (Exception e)
            {
                return $"Could not export '{elementId}': " + e.Message;
            }
        }

        private string ExportIndividualItem(FilePath documentPath, XElement element, FilePath exportDirectory)
        {
            var idAttribute = element.Attribute("id");
            if (idAttribute == null)
                return $"Could not export trophy element: Unknown element id.";

            var elementId = idAttribute.Value;
            try
            {
                var exportFilename = element.InkAttribute("export-filename")?.Value;
                if (exportFilename == null)
                    throw new ArgumentException($"Cannot find 'inkscape:export-filename' attribute for element '{elementId}'.");

                var subdirectoryPath = exportDirectory.Append(Path.GetFileNameWithoutExtension(exportFilename));
                var filenameRoot = elementId.Remove(elementId.Length - "_export".Length);
                var trophyTypeRoot = filenameRoot.Substring(filenameRoot.LastIndexOf('_') + 1);

                var smallPath = subdirectoryPath.Append(trophyTypeRoot + "120.png");
                SvgToPngExporter.ExportPng(documentPath, smallPath, elementId, 96);

                if (trophyTypeRoot != "rank" && trophyTypeRoot != "all")
                {
                    var overallMirrorPath = exportDirectory.Append("Overall").Append(filenameRoot + ".png");
                    SvgToPngExporter.ExportPng(documentPath, overallMirrorPath, elementId, 96);
                }

                var largePath = subdirectoryPath.Append(trophyTypeRoot + "240.png");
                SvgToPngExporter.ExportPng(documentPath, largePath, elementId, 192);

                return $"Element '{elementId}' exported!";
            }
            catch (Exception e)
            {
                return $"Could not export '{elementId}': " + e.Message;
            }
        }
    }
}
