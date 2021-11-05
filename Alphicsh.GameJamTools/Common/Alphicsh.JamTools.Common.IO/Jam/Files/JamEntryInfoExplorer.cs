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
            return GetEntryInfoFromFile(id, entryDirectoryPath)
                ?? StubEntryInfoFromDirectory(id, entryDirectoryPath);
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
            var entryDirectoryName = entryDirectoryPath.GetLastSegmentName();
            var byIndex = entryDirectoryName.LastIndexOf(" by ");

            string title;
            string[] authors;
            if (byIndex == -1)
            {
                title = entryDirectoryName;
                authors = new[] { "??? " };
            }
            else
            {
                title = entryDirectoryName.Remove(byIndex);
                var authorsString = entryDirectoryName.Substring(byIndex + " by ".Length);
                authors = authorsString.Split(",", StringSplitOptions.TrimEntries);
            }

            return new JamEntryInfo()
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
    }
}
