using System.Collections.Generic;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTally.Model.Vote
{
    public class JamVoteCollection
    {
        private static JamVoteSaver VoteSaver { get; } = new JamVoteSaver();

        public FilePath DirectoryPath { get; init; } = default!;
        public IList<JamVote> Votes { get; init; } = default!;

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
    }
}
