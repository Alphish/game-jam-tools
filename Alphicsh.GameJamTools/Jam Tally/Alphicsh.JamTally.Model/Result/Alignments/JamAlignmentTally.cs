using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Alignments
{
    public class JamAlignmentTally
    {
        public IReadOnlyCollection<JamAlignmentVote> Votes { get; init; }
        public IReadOnlyDictionary<JamAlignmentOption, decimal> TotalScores { get; init; }
    }
}
