using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPlayer.IO.Export
{
    public class ExporterInfo
    {
        public string? ReviewsTitle { get; init; }
        public bool? ExportIncompleteRankings { get; init; }
        public string? EntryCommentTemplate { get; init; }

        // ---------------
        // Reading/writing
        // ---------------

        public static ExporterInfo LoadOrGetDefault(FilePath exporterInfoPath)
        {
            var reader = new ExporterInfoFilesReader();

            // try read ranking info from file
            var infoFromFile = reader.TryLoadExporterInfo(exporterInfoPath);
            if (infoFromFile != null)
                return infoFromFile;

            // if ranking info is not available, return the default one
            var defaultInfo = new ExporterInfo();
            return defaultInfo;
        }

        public void SaveTo(FilePath exporterInfoPath)
        {
            var writer = new ExporterInfoFilesWriter();
            writer.SaveExporter(this, exporterInfoPath);
        }
    }
}
