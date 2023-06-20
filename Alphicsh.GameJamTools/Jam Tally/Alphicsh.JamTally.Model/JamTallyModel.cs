using System;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTally.Model.Jam.Loading;

namespace Alphicsh.JamTally.Model
{
    public class JamTallyModel
    {
        internal static JamTallyModel Current { get; set; } = default!;

        private static JamLoader JamLoader { get; } = new JamLoader();

        public JamOverview? Jam { get; private set; }
        public bool HasJam => Jam != null;

        public JamTallyModel()
        {
            if (Current != null)
                throw new InvalidOperationException("AppModel should be created only once.");

            Current = this;
        }

        // -----------
        // Loading Jam
        // -----------

        public void LoadDirectory(FilePath directoryPath)
            => Jam = JamLoader.LoadFromDirectory(directoryPath);
    }
}
