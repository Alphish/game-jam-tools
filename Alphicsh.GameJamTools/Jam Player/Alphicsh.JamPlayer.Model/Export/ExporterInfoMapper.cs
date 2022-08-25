using Alphicsh.JamPlayer.IO.Export;

namespace Alphicsh.JamPlayer.Model.Export
{
    internal class ExporterInfoMapper
    {
        public ExporterInfo MapExporterToInfo(Exporter exporter)
        {
            var options = exporter.Options;
            return new ExporterInfo
            {
                ReviewsTitle = options.ReviewsTitle,
                ExportIncompleteRankings = options.ExportIncompleteRankings,
                EntryCommentTemplate = options.EntryCommentTemplate,
            };
        }

        public Exporter MapInfoToExporter(AppModel appModel, ExporterInfo exporterInfo)
        {
            var exporter = new Exporter(appModel);

            var options = exporter.Options;
            options.ReviewsTitle = exporterInfo.ReviewsTitle ?? options.ReviewsTitle;
            options.ExportIncompleteRankings = exporterInfo.ExportIncompleteRankings ?? options.ExportIncompleteRankings;
            options.EntryCommentTemplate = exporterInfo.EntryCommentTemplate ?? options.EntryCommentTemplate;

            return exporter;
        }
    }
}
