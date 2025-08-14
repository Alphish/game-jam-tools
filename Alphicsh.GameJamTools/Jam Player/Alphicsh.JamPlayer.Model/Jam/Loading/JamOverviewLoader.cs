using Alphicsh.JamTools.Common.IO.Jam;
using Alphicsh.JamTools.Common.IO.Jam.Loading;
using Alphicsh.JamTools.Common.IO.Storage.Loading;

namespace Alphicsh.JamPlayer.Model.Jam.Loading
{
    public class JamOverviewLoader : BaseModelLoader<JamInfo, JamCore, JamOverview>
    {
        private static JamInfoReader JamInfoReader { get; } = new JamInfoReader();
        private static JamInfoToOverviewMapper Mapper { get; } = new JamInfoToOverviewMapper();

        public JamOverviewLoader() : base(JamInfoReader, Mapper, fixBeforeLoading: false)
        {
        }
    }
}
