using Alphicsh.JamTools.Common.IO;
using System.Collections.Generic;
using System.IO;
using Alphicsh.JamTally.Model.Vote.Search;
using Alphicsh.JamTally.Model.Vote.Serialization.Parsing;

namespace Alphicsh.JamTally.Model.Vote
{
    public class JamVoteLoader
    {
        public JamVoteCollection LoadFromDirectory(FilePath directoryPath)
        {
            var tallyPath = directoryPath.Append(".jamtally");
            var votes = LoadVotes(tallyPath);
            var alignmentBattleData = LoadAlignmentBattleData(tallyPath);
            return new JamVoteCollection { DirectoryPath = directoryPath, Votes = votes, AlignmentBattleData = alignmentBattleData };
        }

        private IList<JamVote> LoadVotes(FilePath tallyPath)
        {
            var votesPath = tallyPath.Append("votes.jamvotes");
            if (!votesPath.HasFile())
                return new List<JamVote>();

            var content = File.ReadAllText(votesPath.Value);

            var jam = JamTallyModel.Current.Jam!;
            var jamSearch = new JamSearch(jam);
            var votesParser = new JamVotesFileParser(jam, jamSearch);
            
            var votes = votesParser.ParseVotes(content);
            return votes;
        }

        private JamAlignmentBattleData? LoadAlignmentBattleData(FilePath tallyPath)
        {
            var alignmentPath = tallyPath.Append("alignments.jamalignments");
            if (!alignmentPath.HasFile())
                return null;

            var content = File.ReadAllText(alignmentPath.Value);
            var processor = new JamAlignmentBattleProcessor(content);
            return processor.ReadData();
        }
    }
}
