using System.Collections.Generic;
using System.IO;
using System.Linq;

using Alphicsh.JamTools.Common.IO.Search;

namespace Alphicsh.JamTools.Common.IO.Jam.Files
{
    public class JamInfoExplorer
    {
        private JamFilesReader JamFilesReader { get; }
        private JamEntryInfoExplorer EntryInfoExplorer { get; }

        public JamInfoExplorer()
        {
            JamFilesReader = new JamFilesReader();
            EntryInfoExplorer = new JamEntryInfoExplorer();
        }

        public JamInfo? TryLoadJamInfo(FilePath jamFilePath)
        {
            var jamInfo = JamFilesReader.TryLoadJamInfo(jamFilePath);
            if (jamInfo == null)
                return null;

            var jamDirectoryPath = jamFilePath.GetParentDirectoryPath()!.Value;
            var entriesPath = jamDirectoryPath.Append(jamInfo.EntriesSubpath);
            jamInfo.Entries = LoadEntriesFromStubs(entriesPath, jamInfo.EntriesStubs).ToList();

            return jamInfo;
        }

        public JamInfo FindJamInfo(FilePath jamDirectoryPath)
        {
            var jamInfo = StubJamInfoFromFile(jamDirectoryPath)
                ?? StubJamInfoFromDirectory(jamDirectoryPath);

            var entriesPath = jamDirectoryPath.Append(jamInfo.EntriesSubpath);
            jamInfo.Entries = FindEntriesFromStubs(entriesPath, jamInfo.EntriesStubs).ToList();

            return jamInfo;
        }

        // ------------------
        // Stubbing from file
        // ------------------

        private JamInfo? StubJamInfoFromFile(FilePath jamDirectoryPath)
        {
            var jamInfoPaths = FilesystemSearch.ForFilesIn(jamDirectoryPath)
                .IncludingTopDirectoryOnly()
                .WithExtensions(".jaminfo")
                .FindAll()
                .FoundPaths;

            if (!jamInfoPaths.Any())
                return null;

            var jamInfoPath = jamInfoPaths.First();

            return JamFilesReader.TryLoadJamInfo(jamInfoPath);
        }

        // -----------------------
        // Stubbing from directory
        // -----------------------

        private JamInfo StubJamInfoFromDirectory(FilePath jamDirectoryPath)
        {
            var entriesPath = FindEntriesPath(jamDirectoryPath);
            if (entriesPath == null)
                throw new DirectoryNotFoundException("Could not find the entries subdirectory in the jam directory.");

            var entriesStubs = FindEntriesStubs(entriesPath.Value).ToList();

            return new JamInfo()
            {
                JamInfoPath = jamDirectoryPath.Append("jam.jaminfo"),
                EntriesSubpath = entriesPath.Value.AsRelativeTo(jamDirectoryPath),
                EntriesStubs = entriesStubs,
            };
        }

        private FilePath? FindEntriesPath(FilePath jamDirectoryPath)
        {
            var entriesPaths = FilesystemSearch.ForDirectoriesIn(jamDirectoryPath)
                .IncludingTopDirectoryOnly()
                .FindMatches("Entries")
                .ElseFindMatches("*Entries*")
                .FoundPaths;

            return entriesPaths.Any() ? entriesPaths.First() : null;
        }

        private IEnumerable<JamEntryStub> FindEntriesStubs(FilePath entriesPath)
        {
            var jamEntries = FilesystemSearch.ForDirectoriesIn(entriesPath)
                .IncludingTopDirectoryOnly()
                .FindAll()
                .FoundPaths;

            foreach (var jamEntryPath in jamEntries)
            {
                var subpath = jamEntryPath.AsRelativeTo(entriesPath);
                yield return new JamEntryStub
                {
                    Id = subpath.Value,
                    EntrySubpath = subpath.Value,
                };
            }
        }

        // ---------------
        // Loading entries
        // ---------------

        private IEnumerable<JamEntryInfo> LoadEntriesFromStubs(FilePath entriesPath, IReadOnlyCollection<JamEntryStub> stubs)
        {
            foreach (var stub in stubs)
            {
                var id = stub.Id;
                var entryDirectoryPath = entriesPath.Append(stub.EntrySubpath);
                var entry = EntryInfoExplorer.TryLoadJamEntryInfo(id, entryDirectoryPath);
                if (entry != null)
                    yield return entry;
            }
        }

        private IEnumerable<JamEntryInfo> FindEntriesFromStubs(FilePath entriesPath, IReadOnlyCollection<JamEntryStub> stubs)
        {
            foreach (var stub in stubs)
            {
                var id = stub.Id;
                var entryDirectoryPath = entriesPath.Append(stub.EntrySubpath);
                yield return EntryInfoExplorer.FindJamEntryInfo(id, entryDirectoryPath);
            }
        }
    }
}
