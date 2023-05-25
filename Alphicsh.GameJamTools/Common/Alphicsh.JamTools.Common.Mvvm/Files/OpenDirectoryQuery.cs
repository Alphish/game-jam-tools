using System.IO;
using System.Windows.Forms;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTools.Common.Mvvm.Files
{
    public class OpenDirectoryQuery
    {
        private FolderBrowserDialog Dialog { get; } = new FolderBrowserDialog();

        public FilePath? GetPath()
        {
            var result = Dialog.ShowDialog();
            var hasValidResult = result == DialogResult.OK && Directory.Exists(Dialog.SelectedPath);
            return hasValidResult ? FilePath.From(Dialog.SelectedPath) : null;
        }
    }
}
