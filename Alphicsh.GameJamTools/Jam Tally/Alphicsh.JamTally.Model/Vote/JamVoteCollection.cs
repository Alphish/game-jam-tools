using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Result;
using Alphicsh.JamTally.Model.Result.Calculation;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTally.Model.Vote
{
    public class JamVoteCollection
    {
        private static JamVoteSaver VoteSaver { get; } = new JamVoteSaver();
        private static JamTallyCalculator Calculator { get; } = new JamTallyCalculator();

        public FilePath DirectoryPath { get; init; } = default!;
        public IList<JamVote> Votes { get; init; } = default!;
        public JamAlignmentBattleData? AlignmentBattleData { get; init; }

        public bool HasTallyResult => TallyResult != null;
        public JamTallyResult? TallyResult { get; private set; }
        public JamTallyResultManager? ResultManager { get; private set; }

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
            var applicableVotes = Votes.Where(vote => vote.Ranking.Any()).ToList();
            TallyResult = Calculator.Calculate(JamTallyModel.Current.Jam!, applicableVotes);
            ResultManager = new JamTallyResultManager { Result = TallyResult };
        }
    }
}
