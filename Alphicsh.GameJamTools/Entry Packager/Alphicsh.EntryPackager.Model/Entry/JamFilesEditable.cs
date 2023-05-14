using System.Collections.Generic;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.EntryPackager.Model.Entry
{
    public class JamFilesEditable
    {
        public FilePath DirectoryPath { get; private set; }

        public IList<JamLauncherEditable> Launchers { get; } = new List<JamLauncherEditable>();

        public void SetDirectoryPath(FilePath directoryPath)
        {
            DirectoryPath = directoryPath;
            foreach (var launcher in Launchers)
            {
                launcher.UpdateLaunchData();
            }
        }

        public void AddLauncher()
        {
            Launchers.Add(new JamLauncherEditable(this));
        }
    }
}
