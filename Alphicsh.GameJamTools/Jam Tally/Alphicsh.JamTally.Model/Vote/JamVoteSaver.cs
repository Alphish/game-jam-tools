using System.IO;
using System.Linq;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTally.Model.Vote
{
    public class JamVoteSaver
    {
        public void SaveToDirectory(FilePath directoryPath, JamVoteCollection votesCollection)
        {
            var subdirectoryPath = directoryPath.Append(".jamtally");
            if (!subdirectoryPath.HasDirectory())
                Directory.CreateDirectory(subdirectoryPath.Value);

            var votesPath = subdirectoryPath.Append("votes.jamvotes");
            var voteContent = votesCollection.Votes.Select(vote => vote.Content);
            var content = "### VOTE ###\n" + string.Join("\n### VOTE ###\n", voteContent);
            File.WriteAllText(votesPath.Value, content);
        }
    }
}
