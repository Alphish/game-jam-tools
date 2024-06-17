using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Vote.Serialization.Formatting
{
    internal class JamVotesFileFormatter
    {
        private JamVoteContentBuilder ContentBuilder { get; }
        private JamVoteContentFormatter ContentFormatter { get; }

        public JamVotesFileFormatter(JamOverview jam)
        {
            ContentBuilder = new JamVoteContentBuilder(jam);
            ContentFormatter = new JamVoteContentFormatter();
        }

        public string FormatVotes(IEnumerable<JamVote> votes)
        {
            var voteContents = votes.Select(ContentBuilder.BuildVoteContent).ToList();
            var voteStrings = voteContents.Select(ContentFormatter.Format).ToList();
            return string.Join("\n", voteStrings);
        }
    }
}
