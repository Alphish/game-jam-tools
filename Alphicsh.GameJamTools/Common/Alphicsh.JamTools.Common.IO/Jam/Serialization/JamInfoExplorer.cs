using System.Collections.Generic;
using System.IO;
using System.Linq;

using Alphicsh.JamTools.Common.IO.Search;

namespace Alphicsh.JamTools.Common.IO.Jam.Serialization
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

            var jamDirectoryPath = jamFilePath.GetParentDirectoryPath();
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

        public JamInfo RediscoverJamInfo(FilePath jamDirectoryPath)
        {
            var jamInfo = StubJamInfoFromFile(jamDirectoryPath) ?? StubJamInfoFromDirectory(jamDirectoryPath);

            jamInfo.LogoFileName ??= FindLogoPath(jamDirectoryPath)?.AsRelativeTo(jamDirectoryPath).Value;

            var entriesPath = jamDirectoryPath.Append(jamInfo.EntriesSubpath);
            jamInfo.Entries = RediscoverEntriesFromStubs(entriesPath, jamInfo.EntriesStubs).ToList();

            return jamInfo;
        }

        // ------------------
        // Stubbing from file
        // ------------------

        private JamInfo? StubJamInfoFromFile(FilePath jamDirectoryPath)
        {
            var jamInfoPath = FilesystemSearch.ForFilesIn(jamDirectoryPath)
                .WithExtensions(".jaminfo")
                .FindAll()
                .FirstOrDefault();

            if (jamInfoPath == null)
                return null;

            return JamFilesReader.TryLoadJamInfo(jamInfoPath.Value);
        }

        // -----------------------
        // Stubbing from directory
        // -----------------------

        private JamInfo StubJamInfoFromDirectory(FilePath jamDirectoryPath)
        {
            var logoPath = FindLogoPath(jamDirectoryPath);

            var entriesPath = FindEntriesSubpath(jamDirectoryPath);
            if (entriesPath == null)
                throw new DirectoryNotFoundException("Could not find the entries subdirectory in the jam directory.");

            var entriesStubs = FindEntriesStubs(entriesPath.Value).ToList();

            return new JamInfo()
            {
                JamInfoPath = jamDirectoryPath.Append("jam.jaminfo"),
                LogoFileName = logoPath?.AsRelativeTo(jamDirectoryPath).Value,
                EntriesSubpath = entriesPath.Value.AsRelativeTo(jamDirectoryPath).Value,
                EntriesStubs = entriesStubs,
            };
        }

        private FilePath? FindLogoPath(FilePath jamDirectoryPath)
        {
            return FilesystemSearch.ForFilesIn(jamDirectoryPath)
                .IncludingTopDirectoryOnly()
                .WithExtensions(".png", ".jpg", ".jpeg")
                .FindMatches("logo")
                .ElseFindMatches("logo*")
                .ElseFindMatches("*logo*")
                .FirstOrDefault();
        }

        private FilePath? FindEntriesSubpath(FilePath jamDirectoryPath)
        {
            return FilesystemSearch.ForDirectoriesIn(jamDirectoryPath)
                .FindMatches("Entries")
                .ElseFindMatches("*Entries*")
                .FirstOrDefault();
        }

        private IEnumerable<JamEntryStub> FindEntriesStubs(FilePath entriesPath)
        {
            var jamEntries = FilesystemSearch.ForDirectoriesIn(entriesPath)
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

        private IEnumerable<JamEntryLegacyInfo> LoadEntriesFromStubs(FilePath entriesPath, IReadOnlyCollection<JamEntryStub> stubs)
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

        private IEnumerable<JamEntryLegacyInfo> FindEntriesFromStubs(FilePath entriesPath, IReadOnlyCollection<JamEntryStub> stubs)
        {
            foreach (var stub in stubs)
            {
                var id = stub.Id;
                var entryDirectoryPath = entriesPath.Append(stub.EntrySubpath);
                yield return EntryInfoExplorer.FindJamEntryInfo(id, entryDirectoryPath);
            }
        }

        private IEnumerable<JamEntryLegacyInfo> RediscoverEntriesFromStubs(FilePath entriesPath, IReadOnlyCollection<JamEntryStub> stubs)
        {
            foreach (var stub in stubs)
            {
                var id = stub.Id;
                var entryDirectoryPath = entriesPath.Append(stub.EntrySubpath);
                yield return EntryInfoExplorer.RediscoverJamEntryInfo(id, entryDirectoryPath);
            }
        }
    }
}
