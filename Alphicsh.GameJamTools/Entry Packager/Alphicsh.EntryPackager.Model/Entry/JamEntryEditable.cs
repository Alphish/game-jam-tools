using System;
using System.Text.RegularExpressions;
using Alphicsh.EntryPackager.Model.Entry.Files;

namespace Alphicsh.EntryPackager.Model.Entry
{
    public class JamEntryEditable
    {
        public string Title { get; set; } = default!;
        public string? ShortTitle { get; set; } = default!;
        public string DisplayShortTitle => !string.IsNullOrWhiteSpace(ShortTitle) ? ShortTitle : Title;

        public JamTeamEditable Team { get; } = new JamTeamEditable();
        public JamFilesEditable Files { get; } = new JamFilesEditable();

        public string GetDefaultPackageName()
        {
            var title = !string.IsNullOrEmpty(Title) ? Title : "Title";
            var authors = !string.IsNullOrEmpty(Team.DisplayName) ? Team.DisplayName : "Authors";
            var directoryName = title + " by " + authors;
            var sanitizedName = directoryName.Replace(":", " -");
            sanitizedName = Regex.Replace(sanitizedName, "[^0-9A-Za-z '!,()_-]", "");
            return sanitizedName.Substring(0, Math.Min(sanitizedName.Length, 80));
        }
    }
}
