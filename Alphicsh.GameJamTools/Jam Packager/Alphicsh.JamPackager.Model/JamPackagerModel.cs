using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamPackager.Model.Jam.Loading;
using Alphicsh.JamPackager.Model.Jam;
using Alphicsh.JamPackager.Model.Jam.Compatibility;

namespace Alphicsh.JamPackager.Model
{
    public class JamPackagerModel
    {
        private static JamCompatibilityExporter CompatibilityExporter { get; } = new JamCompatibilityExporter();

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

        public void ExportCompatibilityData()
        {
            if (Jam == null)
                return;

            CompatibilityExporter.Export(Jam);
        }
    }
}
