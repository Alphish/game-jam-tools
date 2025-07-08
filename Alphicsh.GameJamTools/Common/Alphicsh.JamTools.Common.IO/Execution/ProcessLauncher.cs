using System;
using System.ComponentModel;
using System.Diagnostics;

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

            try
            {
                Process.Start(processStartInfo);
            }
            catch (Win32Exception)
            {
                // prevent crashing when user refuses to open the application
            }
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
            {
                OpenWebsite(gxGameUri);
                return;
            }

            var processStartInfo = new ProcessStartInfo(operaGxFullPath.Value, '"' + gxGameUri.ToString() + '"');
            Process.Start(processStartInfo);
        }
    }
}
