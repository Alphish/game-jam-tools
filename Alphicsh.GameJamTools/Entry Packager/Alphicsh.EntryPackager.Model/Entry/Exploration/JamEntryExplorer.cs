using System.Linq;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.EntryPackager.Model.Entry.Exploration
{
    public class JamEntryExplorer
    {
        public void InitFromDirectory(FilePath directoryPath, JamEntryEditable jamEntry)
        {
            jamEntry.Files.SetDirectoryPath(directoryPath);

            FindTitleAndTeam(directoryPath, jamEntry);
        }

        // --------------
        // Title and team
        // --------------

        private void FindTitleAndTeam(FilePath directoryPath, JamEntryEditable jamEntry)
        {
            var directoryName = directoryPath.GetLastSegmentName();
            if (string.IsNullOrWhiteSpace(jamEntry.Title))
                jamEntry.Title = ExtractTitle(directoryName);
            if (!jamEntry.Team.Authors.Any())
                jamEntry.Team.SetAuthorsString(ExtractAuthors(directoryName));
        }

        private string ExtractTitle(string directoryName)
        {
            var lastByIdx = directoryName.LastIndexOf(" by ");
            return lastByIdx >= 0 ? directoryName.Remove(lastByIdx) : directoryName;
        }

        private string ExtractAuthors(string directoryName)
        {
            var lastByIdx = directoryName.LastIndexOf(" by ");
            return lastByIdx >= 0 ? directoryName.Substring(lastByIdx + " by ".Length) : string.Empty;
        }
    }
}
