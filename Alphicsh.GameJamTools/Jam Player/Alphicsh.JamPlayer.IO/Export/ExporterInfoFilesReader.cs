using System;
using System.IO;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.JamPlayer.IO.Export
{
    internal class ExporterInfoFilesReader
    {
        private JsonContentSerializer<ExporterInfo> ExporterInfoSerializer { get; }

        public ExporterInfoFilesReader()
        {
            ExporterInfoSerializer = new JsonContentSerializer<ExporterInfo>();
        }

        public ExporterInfo? TryLoadExporterInfo(FilePath exporterInfoPath)
        {
            if (exporterInfoPath.IsRelative())
                throw new ArgumentException("The exporter info can only be read from the absolute file path.", nameof(exporterInfoPath));

            if (!exporterInfoPath.HasFile())
                return null;

            var content = File.ReadAllText(exporterInfoPath.Value);
            var exporterInfo = ExporterInfoSerializer.Deserialize(content);
            if (exporterInfo == null)
                return null;

            return exporterInfo;
        }
    }
}
