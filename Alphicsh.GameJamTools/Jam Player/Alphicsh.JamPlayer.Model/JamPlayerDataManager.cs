using System.IO;
using Alphicsh.JamPlayer.IO.Export;
using Alphicsh.JamPlayer.IO.Ranking;
using Alphicsh.JamPlayer.Model.Export;
using Alphicsh.JamPlayer.Model.Ranking;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPlayer.Model
{
    public class JamPlayerDataManager
    {
        public AppModel AppModel { get; init; } = default!;
        public FilePath DirectoryPath => AppModel.Jam.DirectoryPath.Append(".jamplayer");

        // --------
        // Rankings
        // --------

        private FilePath RankingPath => DirectoryPath.Append("ranking.jamranking");
        private RankingInfoMapper RankingInfoMapper { get; } = new RankingInfoMapper();

        public void SaveRanking()
        {
            var rankingInfo = RankingInfoMapper.MapRankingToInfo(AppModel.Ranking, AppModel.Awards);
            rankingInfo.SaveTo(RankingPath);
        }

        // --------------
        // Export options
        // --------------

        private FilePath ExporterInfoPath => DirectoryPath.Append("exporter.jamsettings");
        private ExporterInfoMapper ExporterInfoMapper { get; } = new ExporterInfoMapper();

        public void SaveExporter()
        {
            var exporterInfo = ExporterInfoMapper.MapExporterToInfo(AppModel.Exporter);
            exporterInfo.SaveTo(ExporterInfoPath);
        }

        public void LoadExporter()
        {
            var exporterInfo = ExporterInfo.LoadOrGetDefault(ExporterInfoPath);
            AppModel.Exporter = ExporterInfoMapper.MapInfoToExporter(AppModel, exporterInfo);
        }
    }
}
