using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamPackager.Model.Jam.Loading;
using Alphicsh.JamPackager.Model.Jam;

namespace Alphicsh.JamPackager.Model
{
    public class JamPackagerModel
    {
        private JamExplorer JamExplorer { get; } = new JamExplorer();

        public JamEditable? Jam { get; private set; }
        public bool HasJam => Jam != null;

        public JamPackagerModel()
        {
        }

        public void LoadDirectory(FilePath directoryPath)
            => Jam = JamExplorer.LoadFromDirectory(directoryPath);

        public void CloseJam()
            => Jam = null;
    }
}
