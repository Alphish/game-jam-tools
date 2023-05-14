using System;
using System.Text.RegularExpressions;

namespace Alphicsh.EntryPackager.Model.Entry
{
    public class JamEntryEditable
    {
        // ----------
        // Basic data
        // ----------

        public JamEntryEditableData Data { get; set; } = new JamEntryEditableData();

        public string Title { get => Data.Title; set => Data.Title = value; }
        public JamTeamEditable Team { get => Data.Team; }

        // -----
        // Files
        // -----

        public JamFilesEditable Files { get; } = new JamFilesEditable();

        public string? DirectoryName { get; set; }

        public string GetDefaultDirectoryName()
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
