namespace Alphicsh.JamPlayer.Model.Export
{
    public class ExportOptions
    {
        private const string DefaultReviewsTitle = "Reviews";
        private const bool DefaultExportIncompleteRankings = false;
        private const string DefaultEntryCommentTemplate = "[b]{TITLE}[/b] by {TEAM}\n\n{COMMENT}\n";

        public string ReviewsTitle { get; set; }
        public bool ExportIncompleteRankings { get; set; }
        public string EntryCommentTemplate { get; set; }

        public ExportOptions()
        {
            ReviewsTitle = DefaultReviewsTitle;
            ExportIncompleteRankings = DefaultExportIncompleteRankings;
            EntryCommentTemplate = DefaultEntryCommentTemplate;
        }

        public void RestoreDefaults()
        {
            ReviewsTitle = DefaultReviewsTitle;
            ExportIncompleteRankings = DefaultExportIncompleteRankings;
            EntryCommentTemplate = DefaultEntryCommentTemplate;
        }
    }
}
