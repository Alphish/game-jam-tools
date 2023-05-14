using System.IO;
using System.Windows.Forms;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTools.Common.Controls.Files
{
    public class OpenFileQuery
    {
        private OpenFileDialog Dialog { get; } = new OpenFileDialog()
        {
            AutoUpgradeEnabled = true,
            CheckFileExists = true,
            Multiselect = false,
        };

        public FilePath? GetPath()
        {
            var result = Dialog.ShowDialog();
            var hasValidResult = result == DialogResult.OK && File.Exists(Dialog.FileName);
            return hasValidResult ? FilePath.From(Dialog.FileName) : null;
        }

        public OpenFileQuery FromDirectory(FilePath directoryPath, bool restoreDirectory = false)
        {
            Dialog.InitialDirectory = directoryPath.Value;
            Dialog.RestoreDirectory = restoreDirectory;
            return this;
        }

        public OpenFileQuery WithFileType(string pattern, string name)
        {
            if (!name.EndsWith(" (" + pattern + ")"))
                name += " (" + pattern + ")";

            var currentFilter = !string.IsNullOrWhiteSpace(Dialog.Filter) ? Dialog.Filter + "|" : "";
            Dialog.Filter = currentFilter + name + "|" + pattern;
            return this;
        }
    }
}
