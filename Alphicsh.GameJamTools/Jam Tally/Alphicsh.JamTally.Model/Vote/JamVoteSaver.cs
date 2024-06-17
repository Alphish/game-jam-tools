using System.IO;
using Alphicsh.JamTally.Model.Vote.Serialization.Formatting;
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

            var jam = JamTallyModel.Current.Jam!;
            var contentFileFormatter = new JamVotesFileFormatter(jam);
            var content = contentFileFormatter.FormatVotes(votesCollection.Votes);

            var votesPath = subdirectoryPath.Append("votes.jamvotes");
            File.WriteAllText(votesPath.Value, content);
        }
    }
}
