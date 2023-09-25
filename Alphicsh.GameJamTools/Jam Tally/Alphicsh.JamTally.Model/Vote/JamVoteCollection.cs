using System.Collections.Generic;
using Alphicsh.JamTally.Model.Result;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTally.Model.Vote
{
    public class JamVoteCollection
    {
        private static JamVoteSaver VoteSaver { get; } = new JamVoteSaver();
        private static JamTallyCalculator TallyCalculator { get; } = new JamTallyCalculator();

        public FilePath DirectoryPath { get; init; } = default!;
        public IList<JamVote> Votes { get; init; } = default!;

        public bool HasTallyResult => TallyResult != null;
        public JamTallyResult? TallyResult { get; private set; }

        public void AddVote()
        {
            Votes.Add(new JamVote());
        }

        public void RemoveVote(JamVote vote)
        {
            Votes.Remove(vote);
        }

        public void SaveVotes()
        {
            VoteSaver.SaveToDirectory(DirectoryPath, this);
        }

        public void TallyVotes()
        {
            TallyResult = TallyCalculator.CalculateResults(this);
        }
    }
}
