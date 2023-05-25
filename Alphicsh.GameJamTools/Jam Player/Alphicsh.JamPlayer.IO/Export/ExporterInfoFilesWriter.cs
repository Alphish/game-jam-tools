using System.IO;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Serialization;

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
            Directory.CreateDirectory(directoryPath.Value);
            File.WriteAllText(exporterInfoPath.Value, content);
        }
    }
}
