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
            var voteContents = content.Split("### VOTE ###", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var votes = voteContents.Select(content => new JamVote(content)).ToList();
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
