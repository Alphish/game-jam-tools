﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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

            var processStartInfo = new ProcessStartInfo(filePath.Value)
            {
                WorkingDirectory = filePath.GetParentDirectoryPath()!.Value.Value,
                UseShellExecute = true
            };
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

            var processStartInfo = new ProcessStartInfo(filePath.Value) { UseShellExecute = true };
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

            var processStartInfo = new ProcessStartInfo(websiteUri.ToString()) { UseShellExecute = true };
            Process.Start(processStartInfo);
        }

        // --------
        // GX Games
        // --------

        // TODO: Add more general-purpose system for handling non-standard game execution

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
