using System.IO;
using System.Linq;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.IO.Search;

namespace Alphicsh.EntryPackager.Model.Entry.Exploration
{
    public class JamEntryExplorer
    {
        public void InitFromDirectory(FilePath directoryPath, JamEntryEditable jamEntry)
        {
            jamEntry.Files.SetDirectoryPath(directoryPath);

            FindTitleAndTeam(directoryPath, jamEntry);
            FindFiles(directoryPath, jamEntry.Files);
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

        // -----
        // Files
        // -----

        private void FindFiles(FilePath directoryPath, JamFilesEditable jamFiles)
        {
            FindLaunchers(directoryPath, jamFiles);
        }

        private void FindLaunchers(FilePath directoryPath, JamFilesEditable jamFiles)
        {
            var executablePath = FilesystemSearch.ForFilesIn(directoryPath)
                .WithExtensions(".exe")
                .FindAll()
                .FirstOrDefault();

            if (executablePath.HasValue)
            {
                var launcher = jamFiles.AddLauncher();
                launcher.SetName("Exe file");
                launcher.SetType(LaunchType.WindowsExe);
                launcher.SetLocation(executablePath.Value.AsRelativeTo(directoryPath).Value);
            }

            var gxGamePath = FilesystemSearch.ForFilesIn(directoryPath)
                .WithExtensions(".gxgame")
                .FindAll()
                .FirstOrDefault();

            if (gxGamePath != null)
            {
                var launcher = jamFiles.AddLauncher();
                launcher.SetName("GX.games page");
                launcher.SetType(LaunchType.GxGamesLink);

                var uri = File.ReadAllText(gxGamePath.Value.Value);
                launcher.SetLocation(uri);
            }
        }
    }
}
