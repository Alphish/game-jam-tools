using Alphicsh.JamTools.Common.IO.Jam.New;
using Alphicsh.JamTools.Common.IO.Jam.New.Loading;
using Alphicsh.JamTools.Common.IO.Storage.Loading;

namespace Alphicsh.JamPlayer.Model.Jam.Loading
{
    public class JamOverviewLoader : BaseModelLoader<NewJamInfo, NewJamCore, JamOverview>
    {
        private static JamInfoReader JamInfoReader { get; } = new JamInfoReader();
        private static JamInfoToOverviewMapper Mapper { get; } = new JamInfoToOverviewMapper();

        public JamOverviewLoader() : base(JamInfoReader, Mapper, recoverBeforeLoading: false)
        {
        }
    }
}
