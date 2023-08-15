using Alphicsh.JamTools.Common.IO;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

namespace Alphicsh.JamTally.Model.Vote
{
    public class JamVoteLoader
    {
        public JamVoteCollection LoadFromDirectory(FilePath directoryPath)
        {
            var subdirectoryPath = directoryPath.Append(".jamtally");
            var votesPath = subdirectoryPath.Append("votes.jamvotes");
            if (!votesPath.HasFile())
                return new JamVoteCollection { DirectoryPath = directoryPath, Votes = new List<JamVote>() };

            var content = File.ReadAllText(votesPath.Value);

            var voteContents = content.Split("### VOTE ###", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var votes = voteContents.Select(content => new JamVote(content)).ToList();
            return new JamVoteCollection { DirectoryPath = directoryPath, Votes = votes };
        }
    }
}
