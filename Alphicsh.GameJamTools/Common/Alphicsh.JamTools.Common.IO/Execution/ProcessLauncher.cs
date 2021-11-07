using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Alphicsh.JamTools.Common.IO.Execution
{
    public class ProcessLauncher
    {
        // -----
        // Files
        // -----

        public bool CanOpenFile(FilePath filePath)
        {
            return filePath.HasFile();
        }

        public void OpenFile(FilePath filePath)
        {
            if (!CanOpenFile(filePath))
                return;

            var processStartInfo = new ProcessStartInfo(filePath.Value);
            processStartInfo.WorkingDirectory = filePath.GetParentDirectoryPath()!.Value.Value;
            Process.Start(processStartInfo);
        }

        // -----------
        // Directories
        // -----------

        public bool CanOpenDirectory(FilePath filePath)
        {
            return filePath.HasDirectory();
        }

        public void OpenDirectory(FilePath filePath)
        {
            if (!CanOpenDirectory(filePath))
                return;

            var processStartInfo = new ProcessStartInfo(filePath.Value);
            Process.Start(processStartInfo);
        }

        // --------
        // Websites
        // --------

        private static HashSet<string> ValidWebsiteSchemes { get; }
            = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "http", "https" };

        public bool CanOpenWebsite(Uri websiteUri)
        {
            return ValidWebsiteSchemes.Contains(websiteUri.Scheme);
        }

        public void OpenWebsite(Uri websiteUri)
        {
            if (!CanOpenWebsite(websiteUri))
                return;

            var processStartInfo = new ProcessStartInfo(websiteUri.ToString());
            Process.Start(processStartInfo);
        }
    }
}
