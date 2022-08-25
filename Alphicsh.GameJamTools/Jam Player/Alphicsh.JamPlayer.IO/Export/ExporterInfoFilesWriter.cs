using System.IO;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPlayer.IO.Export
{
    internal class ExporterInfoFilesWriter
    {
        private JsonContentSerializer<ExporterInfo> ExporterInfoSerializer { get; }

        public ExporterInfoFilesWriter()
        {
            ExporterInfoSerializer = new JsonContentSerializer<ExporterInfo>();
        }

        public void SaveExporter(ExporterInfo exporterInfo, FilePath exporterInfoPath)
        {
            var content = ExporterInfoSerializer.Serialize(exporterInfo);
            var directoryPath = exporterInfoPath.GetParentDirectoryPath();
            Directory.CreateDirectory(directoryPath!.Value.Value);
            File.WriteAllText(exporterInfoPath.Value, content);
        }
    }
}
