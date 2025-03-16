using System.Collections.Generic;
using Alphicsh.JamTally.Model.Result;
using Alphicsh.JamTally.Model.Result.Calculation;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTally.Model.Vote
{
    public class JamVoteCollection
    {
        private static JamVoteSaver VoteSaver { get; } = new JamVoteSaver();
        private static JamTallyCalculator TallyCalculator { get; } = new JamTallyCalculator();
        private static JamTallyNewCalculator NewCalculator { get; } = new JamTallyNewCalculator();

        public FilePath DirectoryPath { get; init; } = default!;
        public IList<JamVote> Votes { get; init; } = default!;
        public JamAlignmentBattleData? AlignmentBattleData { get; init; }

        public bool HasTallyResult => TallyResult != null;
        public JamTallyResult? TallyResult { get; private set; }
        public JamTallyNewResult? NewTallyResult { get; private set; }

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
            TallyResult.VoteCollection = this;

            NewTallyResult = NewCalculator.Calculate(JamTallyModel.Current.Jam!, this);
        }
    }
}
