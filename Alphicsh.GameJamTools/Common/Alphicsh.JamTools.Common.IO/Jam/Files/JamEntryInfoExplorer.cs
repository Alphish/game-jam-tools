using System;
using System.Linq;

using Alphicsh.JamTools.Common.IO.Search;

namespace Alphicsh.JamTools.Common.IO.Jam.Files
{
    public class JamEntryInfoExplorer
    {
        private JamEntryFilesReader EntryFilesReader { get; }

        public JamEntryInfoExplorer()
        {
            EntryFilesReader = new JamEntryFilesReader();
        }

        public JamEntryInfo? TryLoadJamEntryInfo(string id, FilePath entryDirectoryPath)
        {
            return GetEntryInfoFromFile(id, entryDirectoryPath);
        }

        public JamEntryInfo FindJamEntryInfo(string id, FilePath entryDirectoryPath)
        {
            var fileBasedInfo = GetEntryInfoFromFile(id, entryDirectoryPath);
            if (fileBasedInfo != null)
                return fileBasedInfo;

            var directoryBasedInfo = FindJamEntryInfo(id, entryDirectoryPath);
            RediscoverJamEntryFiles(directoryBasedInfo);
            return directoryBasedInfo;
        }

        public JamEntryInfo RediscoverJamEntryInfo(string id, FilePath entryDirectoryPath)
        {
            var jamEntryInfo = GetEntryInfoFromFile(id, entryDirectoryPath)
                ?? FindJamEntryInfo(id, entryDirectoryPath);

            RediscoverJamEntryFiles(jamEntryInfo);
            return jamEntryInfo;
        }

        // -----------------
        // Loading from file
        // -----------------

        private JamEntryInfo? GetEntryInfoFromFile(string id, FilePath entryDirectoryPath)
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

        private JamEntryInfo StubEntryInfoFromDirectory(string id, FilePath entryDirectoryPath)
        {
            var titleAndAuthors = ExtractTitleAndAuthors(entryDirectoryPath);

            return new JamEntryInfo()
            {
                Id = id,
                EntryInfoPath = entryDirectoryPath.Append("entry.jamentry"),
                Title = titleAndAuthors.Title,
                Team = new JamTeamInfo
                {
                    Name = null,
                    Authors = titleAndAuthors.Authors.Select(name => new JamAuthorInfo { Name = name }).ToList()
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

        private void RediscoverJamEntryFiles(JamEntryInfo jamEntryInfo)
        {
            var entryDirectoryPath = jamEntryInfo.EntryDirectoryPath;

            var gamePath = FindGamePath(entryDirectoryPath);

            var bigThumbnailPath = FindBigThumbnailPath(entryDirectoryPath);
            var smallThumbnailPath = FindSmallThumbnailPath(entryDirectoryPath);
            bigThumbnailPath = bigThumbnailPath ?? smallThumbnailPath;
            smallThumbnailPath = smallThumbnailPath ?? bigThumbnailPath;

            jamEntryInfo.GameFileName ??= gamePath?.GetLastSegmentName();
            jamEntryInfo.ThumbnailFileName ??= bigThumbnailPath?.GetLastSegmentName();
            jamEntryInfo.ThumbnailSmallFileName ??= smallThumbnailPath?.GetLastSegmentName();
        }

        private FilePath? FindGamePath(FilePath entryDirectoryPath)
        {
            return FilesystemSearch.ForFilesIn(entryDirectoryPath)
                .WithExtensions(".exe")
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
    }
}
