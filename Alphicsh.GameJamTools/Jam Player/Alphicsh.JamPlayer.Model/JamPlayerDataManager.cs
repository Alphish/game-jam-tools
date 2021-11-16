using Alphicsh.JamTools.Common.IO;

using Alphicsh.JamPlayer.IO.Ranking;
using Alphicsh.JamPlayer.Model.Ranking;

namespace Alphicsh.JamPlayer.Model
{
    public class JamPlayerDataManager
    {
        public AppModel AppModel { get; init; } = default!;
        public FilePath DirectoryPath => AppModel.Jam.DirectoryPath.Append(".jamplayer");

        // --------
        // Rankings
        // --------

        public FilePath RankingPath => DirectoryPath.Append("ranking.jamranking");
        private RankingInfoMapper RankingInfoMapper { get; } = new RankingInfoMapper();

        public void SaveRanking()
        {
            var rankingInfo = RankingInfoMapper.MapRankingToInfo(AppModel.Ranking);
            rankingInfo.SaveTo(RankingPath);
        }
    }
}
