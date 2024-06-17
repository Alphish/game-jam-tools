using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Vote.Management
{
    public class JamVoteManager
    {
        public void AutoFillVoteAuthoredEntries(JamVote vote, JamOverview jam)
        {
            var authored = jam.Search!.FindEntriesBy(vote.Voter);
            vote.SetAuthored(authored);
            vote.RecalculateMissingEntries(jam.Entries);
        }
    }
}
