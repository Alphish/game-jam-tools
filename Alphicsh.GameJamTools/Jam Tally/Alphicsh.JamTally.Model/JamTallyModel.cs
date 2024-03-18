using System;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTally.Model.Jam.Loading;
using Alphicsh.JamTally.Model.Vote;
using Alphicsh.JamTally.Model.Vote.Management;

namespace Alphicsh.JamTally.Model
{
    public class JamTallyModel
    {
        internal static JamTallyModel Current { get; set; } = default!;

        private static JamLoader JamLoader { get; } = new JamLoader();
        private static JamVoteLoader VoteLoader { get; } = new JamVoteLoader();

        public bool HasJam => Jam != null;
        public bool HasAlignments => Jam?.Alignments != null;
        public JamOverview? Jam { get; private set; }
        public JamVoteCollection? VotesCollection { get; private set; }

        public JamVoteManager VoteManager { get; } = new JamVoteManager();

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
        {
            Jam = JamLoader.LoadFromDirectory(directoryPath);
            VotesCollection = VoteLoader.LoadFromDirectory(directoryPath);
        }
    }
}
