using System;
using System.Collections.Generic;
using System.Linq;
using Alphicsh.EntryPackager.Model.Entry.Exploration;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.EntryPackager.Model.Entry.Files
{
    public class JamFilesEditable
    {
        private JamEntryUnknownDataFinder DataFinder { get; } = new JamEntryUnknownDataFinder();

        public FilePath DirectoryPath { get; private set; }

        public IList<JamLauncherEditable> Launchers { get; }
        public JamReadmeEditable Readme { get; }
        public JamAfterwordEditable Afterword { get; }
        public JamThumbnailsEditable Thumbnails { get; }

        public JamFilesEditable()
        {
            Launchers = new List<JamLauncherEditable>();
            Readme = new JamReadmeEditable(this);
            Afterword = new JamAfterwordEditable(this);
            Thumbnails = new JamThumbnailsEditable(this);
        }

        public void SetDirectoryPath(FilePath directoryPath)
        {
            DirectoryPath = directoryPath;
            foreach (var launcher in Launchers)
            {
                launcher.UpdateLaunchData();
            }
        }

        public JamLauncherEditable AddLauncher()
        {
            var launcher = new JamLauncherEditable(this);
            Launchers.Add(launcher);
            return launcher;
        }

        public bool HasLauncherWithLocation(string location)
        {
            return Launchers.Any(launcher => StringComparer.OrdinalIgnoreCase.Equals(launcher.Location, location));
        }

        public void ClearInvalid()
        {
            var launchersToRemove = Launchers.Where(launcher => !launcher.CanLaunch).ToList();
            foreach (var launcher in launchersToRemove)
            {
                Launchers.Remove(launcher);
            }

            Readme.ClearInvalid();
            Afterword.ClearInvalid();
            Thumbnails.ClearInvalid();
        }

        public void Rediscover()
        {
            DataFinder.RediscoverFiles(this);
        }
    }
}
