using System.Collections.Generic;
using Alphicsh.JamTools.Common.IO.Storage.Loading;

namespace Alphicsh.JamPlayer.Model.Jam.Loading
{
    public class JamLoadModel : LoadModel<JamOverview>
    {
        private static JamOverviewLoader ModelLoader { get; } = new JamOverviewLoader();

        private static JamOverview BlankModel { get; } = new JamOverview
        {
            AwardCriteria = new List<JamAwardCriterion>(),
            Entries = new List<JamEntry>(),
        };

        public JamLoadModel() : base(ModelLoader, BlankModel)
        {
        }
    }
}
