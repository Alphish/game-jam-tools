using System.Collections.Generic;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.EntryPackager.Model.Entry.Files
{
    public class JamFilesEditable
    {
        public FilePath DirectoryPath { get; private set; }

        public IList<JamLauncherEditable> Launchers { get; }
        public JamReadmeEditable Readme { get; }
        public JamAfterwordEditable Afterword { get; }

        public JamFilesEditable()
        {
            Launchers = new List<JamLauncherEditable>();
            Readme = new JamReadmeEditable(this);
            Afterword = new JamAfterwordEditable(this);
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
    }
}
