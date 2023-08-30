using Alphicsh.EntryPackager.Model.Entry.Files;

namespace Alphicsh.EntryPackager.Model.Entry
{
    public class JamEntryEditable
    {
        public string? WrittenBy { get; set; }

        public string Title { get; set; } = default!;
        public string? ShortTitle { get; set; }
        public string? Alignment { get; set; }
        public string DisplayShortTitle => !string.IsNullOrWhiteSpace(ShortTitle) ? ShortTitle : Title;

        public JamTeamEditable Team { get; } = new JamTeamEditable();
        public JamFilesEditable Files { get; } = new JamFilesEditable();
    }
}
