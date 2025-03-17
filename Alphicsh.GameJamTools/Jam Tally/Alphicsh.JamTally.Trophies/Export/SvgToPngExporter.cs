using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTally.Trophies.Export
{
    internal class SvgToPngExporter
    {
        public void ExportPng(FilePath sourcePath, FilePath destinationPath, string elementId, int dpi)
        {
            Directory.CreateDirectory(destinationPath.GetParentDirectoryPath().Value);

            var exportFilenameArg = $"--export-filename=\"{destinationPath.Value}\"";
            var exportIdArg = $"--export-id=\"{elementId}\"";
            var exportDpiArg = $"--export-dpi={dpi}";
            var args = $"{exportFilenameArg} {exportIdArg} {exportDpiArg} \"{sourcePath.Value}\"";
            var executeText = Execute(args);

            if (!File.Exists(destinationPath.Value))
                throw new InvalidOperationException($"For some reason, the export command didn't work:\n{executeText}");
        }

        private string Execute(string args)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "inkscape",
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();

            var resultBuilder = new StringBuilder();
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                resultBuilder.AppendLine(line);
            }

            process.WaitForExit();

            return resultBuilder.ToString();
        }
    }
}
