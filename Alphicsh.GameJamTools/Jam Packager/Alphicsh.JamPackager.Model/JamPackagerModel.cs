using System.Threading.Tasks;
using Alphicsh.JamPackager.Model.Jam;
using Alphicsh.JamPackager.Model.Jam.Compatibility;
using Alphicsh.JamPackager.Model.Jam.Loading;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPackager.Model
{
    public class JamPackagerModel
    {
        private static JamCompatibilityExporter CompatibilityExporter { get; } = new JamCompatibilityExporter();

        private static JamExplorer JamExplorer { get; } = new JamExplorer();

        public JamEditable? Jam { get; private set; }
        public bool HasJam => Jam != null;

        public async Task LoadDirectory(FilePath directoryPath)
            => Jam = await JamExplorer.LoadFromDirectory(directoryPath);

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
