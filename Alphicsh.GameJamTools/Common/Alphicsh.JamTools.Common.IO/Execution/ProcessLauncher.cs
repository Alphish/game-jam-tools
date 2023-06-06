using System;
using System.Diagnostics;
using System.IO;

namespace Alphicsh.JamTools.Common.IO.Execution
{
    public class ProcessLauncher
    {
        public void OpenFile(FilePath filePath)
        {
            var processStartInfo = new ProcessStartInfo(filePath.Value)
            {
                WorkingDirectory = filePath.GetParentDirectoryPath().Value,
                UseShellExecute = true
            };
            Process.Start(processStartInfo);
        }

        public void OpenDirectory(FilePath filePath)
        {
            var processStartInfo = new ProcessStartInfo(filePath.Value) { UseShellExecute = true };
            Process.Start(processStartInfo);
        }

        public void OpenWebsite(Uri websiteUri)
        {
            var processStartInfo = new ProcessStartInfo(websiteUri.ToString()) { UseShellExecute = true };
            Process.Start(processStartInfo);
        }

        public void OpenGxGame(string operaGxPath, Uri gxGameUri)
        {
            var operaGxFullPath = new FilePath(Environment.ExpandEnvironmentVariables(operaGxPath));
            if (!operaGxFullPath.HasFile())
                return;

            var processStartInfo = new ProcessStartInfo(operaGxFullPath.Value, '"' + gxGameUri.ToString() + '"');
            Process.Start(processStartInfo);
        }

        [Obsolete]
        public void OpenGxGame(string operaGxPath, FilePath filePath)
        {
            var operaGxFullPath = new FilePath(Environment.ExpandEnvironmentVariables(operaGxPath));
            if (!operaGxFullPath.HasFile())
                return;

            if (!filePath.HasFile())
                return;

            var gxGameLink = File.ReadAllText(filePath.Value);
            if (!Uri.TryCreate(gxGameLink, UriKind.Absolute, out var gxGameUri))
                return;

            if (gxGameUri.Scheme != "https")
                return;

            var processStartInfo = new ProcessStartInfo(operaGxFullPath.Value, '"' + gxGameLink.ToString() + '"');
            Process.Start(processStartInfo);
        }
    }
}
