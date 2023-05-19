using System;
using System.Linq;

using Alphicsh.JamTools.Common.IO.Search;

namespace Alphicsh.JamTools.Common.IO.Jam.Serialization
{
    public class JamEntryInfoExplorer
    {
        private JamEntryLegacyFilesReader EntryFilesReader { get; }

        public JamEntryInfoExplorer()
        {
            EntryFilesReader = new JamEntryLegacyFilesReader();
        }

        public JamEntryLegacyInfo? TryLoadJamEntryInfo(string id, FilePath entryDirectoryPath)
        {
            return GetEntryInfoFromFile(id, entryDirectoryPath);
        }

        public JamEntryLegacyInfo FindJamEntryInfo(string id, FilePath entryDirectoryPath)
        {
            var fileBasedInfo = GetEntryInfoFromFile(id, entryDirectoryPath);
            if (fileBasedInfo != null)
                return fileBasedInfo;

            var directoryBasedInfo = StubEntryInfoFromDirectory(id, entryDirectoryPath);
            RediscoverJamEntryFiles(directoryBasedInfo);
            return directoryBasedInfo;
        }

        public JamEntryLegacyInfo RediscoverJamEntryInfo(string id, FilePath entryDirectoryPath)
        {
            var jamEntryInfo = GetEntryInfoFromFile(id, entryDirectoryPath)
                ?? FindJamEntryInfo(id, entryDirectoryPath);

            RediscoverJamEntryFiles(jamEntryInfo);
            return jamEntryInfo;
        }

        // -----------------
        // Loading from file
        // -----------------

        private JamEntryLegacyInfo? GetEntryInfoFromFile(string id, FilePath entryDirectoryPath)
        {
            var jamEntryPaths = FilesystemSearch.ForFilesIn(entryDirectoryPath)
                .IncludingTopDirectoryOnly()
                .WithExtensions(".jamentry")
                .FindAll()
                .FoundPaths;

            if (!jamEntryPaths.Any())
                return null;

            var entryInfoPath = jamEntryPaths.First();
            return EntryFilesReader.TryLoadJamEntryInfo(id, entryInfoPath);
        }

        // -----------------------
        // Stubbing from directory
        // -----------------------

        private JamEntryLegacyInfo StubEntryInfoFromDirectory(string id, FilePath entryDirectoryPath)
        {
            var (title, authors) = ExtractTitleAndAuthors(entryDirectoryPath);

            return new JamEntryLegacyInfo()
            {
                Id = id,
                EntryInfoPath = entryDirectoryPath.Append("entry.jamentry"),
                Title = title,
                Team = new JamTeamInfo
                {
                    Name = null,
                    Authors = authors.Select(name => new JamAuthorInfo { Name = name }).ToList()
                }
            };
        }

        private (string Title, string[] Authors) ExtractTitleAndAuthors(FilePath entryDirectoryPath)
        {
            var entryDirectoryName = entryDirectoryPath.GetLastSegmentName();
            var byIndex = entryDirectoryName.LastIndexOf(" by ");

            string title;
            string[] authors;
            if (byIndex == -1)
            {
                return (entryDirectoryName, new[] { "???" });
            }
            else
            {
                title = entryDirectoryName.Remove(byIndex);
                var authorsString = entryDirectoryName.Substring(byIndex + " by ".Length);
                authors = authorsString.Split(",", StringSplitOptions.TrimEntries);
                return (title, authors);
            }
        }

        private void RediscoverJamEntryFiles(JamEntryLegacyInfo jamEntryInfo)
        {
            var entryDirectoryPath = jamEntryInfo.EntryDirectoryPath;

            var gamePath = FindGamePath(entryDirectoryPath);
            jamEntryInfo.GameFileName ??= gamePath?.GetLastSegmentName();

            var bigThumbnailPath = FindBigThumbnailPath(entryDirectoryPath);
            var smallThumbnailPath = FindSmallThumbnailPath(entryDirectoryPath);
            bigThumbnailPath ??= smallThumbnailPath;
            smallThumbnailPath ??= bigThumbnailPath;
            jamEntryInfo.ThumbnailFileName ??= bigThumbnailPath?.GetLastSegmentName();
            jamEntryInfo.ThumbnailSmallFileName ??= smallThumbnailPath?.GetLastSegmentName();

            var readmePath = FindReadmePath(entryDirectoryPath);
            var afterwordPath = FindAfterwordPath(entryDirectoryPath);
            jamEntryInfo.ReadmeFileName ??= readmePath?.GetLastSegmentName();
            jamEntryInfo.IsReadmePlease = IsReadmePleaseFileName(jamEntryInfo.ReadmeFileName);
            jamEntryInfo.AfterwordFileName ??= afterwordPath?.GetLastSegmentName();
        }

        private FilePath? FindGamePath(FilePath entryDirectoryPath)
        {
            return FilesystemSearch.ForFilesIn(entryDirectoryPath)
                .WithExtensions(".exe", ".gxgame")
                .FindAll()
                .FirstOrDefault();
        }

        private FilePath? FindBigThumbnailPath(FilePath entryDirectoryPath)
        {
            return FilesystemSearch.ForFilesIn(entryDirectoryPath)
                .WithExtensions(".png", ".jpg", ".jpeg")
                .ExcludingPatterns("*small*", "*little*", "*tiny*")
                .FindMatches("thumbnail*")
                .ElseFindMatches("thumb*")
                .ElseFindMatches("*thumbnail*")
                .ElseFindMatches("*thumb*")
                .FirstOrDefault();
        }

        private FilePath? FindSmallThumbnailPath(FilePath entryDirectoryPath)
        {
            return FilesystemSearch.ForFilesIn(entryDirectoryPath)
                .WithExtensions(".png", ".jpg", ".jpeg")
                .RequiringPatterns("*small*", "*little*", "*tiny*")
                .FindMatches("thumbnail*")
                .ElseFindMatches("thumb*")
                .ElseFindMatches("*thumbnail*")
                .ElseFindMatches("*thumb*")
                .FirstOrDefault();
        }

        private FilePath? FindReadmePath(FilePath entryDirectoryPath)
        {
            return FilesystemSearch.ForFilesIn(entryDirectoryPath)
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

        private bool IsReadmePleaseFileName(string? filename)
        {
            if (filename == null)
                return false;

            return filename.Contains("please", StringComparison.OrdinalIgnoreCase)
                || filename.Contains("important", StringComparison.OrdinalIgnoreCase);
        }

        private FilePath? FindAfterwordPath(FilePath entryDirectoryPath)
        {
            return FilesystemSearch.ForFilesIn(entryDirectoryPath)
                .IncludingTopDirectoryOnly()
                .FindMatches("*afterword*")
                .FirstOrDefault();
        }
    }
}
