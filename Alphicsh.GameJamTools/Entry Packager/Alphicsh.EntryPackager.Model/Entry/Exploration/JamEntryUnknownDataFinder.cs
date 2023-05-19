using System;
using System.Linq;
using Alphicsh.EntryPackager.Model.Entry.Files;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.IO.Search;
using Alphicsh.JamTools.Common.IO;
using System.IO;

namespace Alphicsh.EntryPackager.Model.Entry.Exploration
{
    public class JamEntryUnknownDataFinder
    {
        public JamEntryEditable FindEntryData(FilePath directoryPath)
        {
            var jamEntry = new JamEntryEditable();

            jamEntry.Files.SetDirectoryPath(directoryPath);
            FindTitleAndTeam(directoryPath, jamEntry);
            FindFilesFor(jamEntry.Files);
            
            return jamEntry;
        }

        public void RediscoverFiles(JamFilesEditable jamFiles)
        {
            FindFilesFor(jamFiles);
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

        private void FindFilesFor(JamFilesEditable jamFiles)
        {
            FindExecutables(jamFiles);
            FindGxGamePages(jamFiles);
            FindReadfiles(jamFiles);
            FindThumbnails(jamFiles);
        }

        // Launchers

        private void FindExecutables(JamFilesEditable jamFiles)
        {
            var executablePaths = FilesystemSearch.ForFilesIn(jamFiles.DirectoryPath)
                .IncludingTopDirectoryOnly()
                .WithExtensions(".exe")
                .FindAll()
                .FoundPaths;

            var hasOneExecutable = executablePaths.Count == 1;
            foreach (var path in executablePaths)
            {
                var locationSubpath = path.AsRelativeTo(jamFiles.DirectoryPath);
                if (jamFiles.HasLauncherWithLocation(locationSubpath.Value))
                    continue;

                var launcher = jamFiles.AddLauncher();
                launcher.SetName(hasOneExecutable ? "Windows Executable" : locationSubpath.GetNameWithoutExtension());
                launcher.SetType(LaunchType.WindowsExe);
                launcher.SetLocation(locationSubpath.Value);
            }
        }

        private void FindGxGamePages(JamFilesEditable jamFiles)
        {
            var gxGamePaths = FilesystemSearch.ForFilesIn(jamFiles.DirectoryPath)
                .IncludingTopDirectoryOnly()
                .WithExtensions(".gxgame")
                .FindAll()
                .FoundPaths;

            var hasOnePage = gxGamePaths.Count == 1;
            foreach (var path in gxGamePaths)
            {
                var location = File.ReadAllText(path.Value);
                if (!Uri.TryCreate(location, UriKind.Absolute, out var uri))
                    continue;

                if (jamFiles.HasLauncherWithLocation(location))
                    continue;

                var launcher = jamFiles.AddLauncher();
                launcher.SetName(hasOnePage ? "GX.games page" : path.GetNameWithoutExtension());
                launcher.SetType(LaunchType.GxGamesLink);
                launcher.SetLocation(location);
            }
        }

        // Readfiles

        private void FindReadfiles(JamFilesEditable jamFiles)
        {
            if (string.IsNullOrWhiteSpace(jamFiles.Readme.Location))
            {
                var readmePath = FindReadmePath(jamFiles.DirectoryPath);
                jamFiles.Readme.Location = readmePath?.AsRelativeTo(jamFiles.DirectoryPath).Value;
                jamFiles.Readme.IsRequired = IsReadmeRequired(readmePath);
            }

            if (string.IsNullOrWhiteSpace(jamFiles.Afterword.Location))
            {
                var afterwordPath = FindAfterwordPath(jamFiles.DirectoryPath);
                jamFiles.Afterword.Location = afterwordPath?.AsRelativeTo(jamFiles.DirectoryPath).Value;
            }
        }

        private FilePath? FindReadmePath(FilePath directoryPath)
        {
            return FilesystemSearch.ForFilesIn(directoryPath)
                .IncludingTopDirectoryOnly()
                .FindMatches("*readme*please*")
                .ElseFindMatches("*readme*important*")
                .ElseFindMatches("*readme*")
                .ElseFindMatches("*read*please*")
                .ElseFindMatches("*read*important*")
                .ElseFindMatches("*read*")
                .ElseFindMatches("*credits*")
                .FirstOrDefault();
        }

        private bool IsReadmeRequired(FilePath? readmePath)
        {
            if (readmePath == null)
                return false;

            var filename = readmePath.Value.Value;
            return filename.Contains("please", StringComparison.OrdinalIgnoreCase)
                || filename.Contains("important", StringComparison.OrdinalIgnoreCase);
        }

        private FilePath? FindAfterwordPath(FilePath directoryPath)
        {
            return FilesystemSearch.ForFilesIn(directoryPath)
                .IncludingTopDirectoryOnly()
                .FindMatches("*afterword*")
                .FirstOrDefault();
        }

        // Thumbnails

        private void FindThumbnails(JamFilesEditable jamFiles)
        {
            if (!jamFiles.Thumbnails.HasThumbnailLocation)
            {
                var thumbnailPath = FindThumbnailPath(jamFiles.DirectoryPath);
                jamFiles.Thumbnails.ThumbnailLocation = thumbnailPath?.AsRelativeTo(jamFiles.DirectoryPath).Value;
            }

            if (!jamFiles.Thumbnails.HasThumbnailSmallLocation)
            {
                var thumbnailSmallPath = FindThumbnailSmallPath(jamFiles.DirectoryPath);
                jamFiles.Thumbnails.ThumbnailSmallLocation = thumbnailSmallPath?.AsRelativeTo(jamFiles.DirectoryPath).Value;
            }
        }

        private FilePath? FindThumbnailPath(FilePath directoryPath)
        {
            return FilesystemSearch.ForFilesIn(directoryPath)
                .WithExtensions(".png", ".jpg", ".jpeg")
                .ExcludingPatterns("*small*", "*little*", "*tiny*")
                .FindMatches("thumbnail*")
                .ElseFindMatches("thumb*")
                .ElseFindMatches("*thumbnail*")
                .ElseFindMatches("*thumb*")
                .FirstOrDefault();
        }

        private FilePath? FindThumbnailSmallPath(FilePath directoryPath)
        {
            return FilesystemSearch.ForFilesIn(directoryPath)
                .WithExtensions(".png", ".jpg", ".jpeg")
                .RequiringPatterns("*small*", "*little*", "*tiny*")
                .FindMatches("thumbnail*")
                .ElseFindMatches("thumb*")
                .ElseFindMatches("*thumbnail*")
                .ElseFindMatches("*thumb*")
                .FirstOrDefault();
        }
    }
}
