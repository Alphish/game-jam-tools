using Alphicsh.JamPlayer.IO.Export;
using Alphicsh.JamPlayer.Model.Export;
using Alphicsh.JamPlayer.Model.Feedback.Storage;
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
        private FeedbackSaver FeedbackSaver { get; } = new FeedbackSaver();

        public async void SaveRanking()
        {
            var batch = await FeedbackSaver.PrepareFileBatchAsync(AppModel.Feedback);
            await FeedbackSaver.SaveFileBatch(batch, batch);
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
