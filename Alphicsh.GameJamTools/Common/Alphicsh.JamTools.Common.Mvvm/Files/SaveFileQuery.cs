using Alphicsh.JamTools.Common.IO;
using System.Windows.Forms;

namespace Alphicsh.JamTools.Common.Mvvm.Files
{
    public class SaveFileQuery
    {
        private SaveFileDialog Dialog { get; } = new SaveFileDialog()
        {
            AutoUpgradeEnabled = true,
        };

        public FilePath? GetPath()
        {
            var result = Dialog.ShowDialog();
            var hasValidResult = result == DialogResult.OK;
            return hasValidResult ? FilePath.From(Dialog.FileName) : null;
        }

        public SaveFileQuery FromDirectory(FilePath directoryPath, bool restoreDirectory = false)
        {
            Dialog.InitialDirectory = directoryPath.Value;
            Dialog.RestoreDirectory = restoreDirectory;
            return this;
        }

        public SaveFileQuery WithFileType(string pattern, string name)
        {
            if (!name.EndsWith(" (" + pattern + ")"))
                name += " (" + pattern + ")";

            var currentFilter = !string.IsNullOrWhiteSpace(Dialog.Filter) ? Dialog.Filter + "|" : "";
            Dialog.Filter = currentFilter + name + "|" + pattern;
            return this;
        }

        public SaveFileQuery WithDefaultName(string name)
        {
            Dialog.FileName = name;
            return this;
        }
    }
}
