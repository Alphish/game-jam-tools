using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Vote
{
    public class JamAlignmentBattleData
    {
        public JamAlignmentOption? ThemeGuessed { get; init; }
        public IReadOnlyCollection<JamEntry> Duplicates { get; init; }
    }
}
