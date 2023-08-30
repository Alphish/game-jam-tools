using System.Collections.Generic;
using System.IO;
using System.Linq;
using Alphicsh.EntryPackager.Model.Entry.Loading;
using Alphicsh.EntryPackager.Model.Entry.Saving;
using Alphicsh.EntryPackager.Model.Entry;
using Alphicsh.JamTools.Common.IO.Compression;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPackager.Model.Jam.Loading
{
    public class JamEntriesExplorer
    {
        private static ZipExtractor EntryExtractor { get; } = new ZipExtractor();
        private static JamEntryExplorer EntryExplorer { get; } = new JamEntryExplorer();

        public IReadOnlyCollection<JamEntryEditable> FindEntries(FilePath entriesPath)
        {
            var entriesDirectory = entriesPath.GetDirectory();
            ExtractEntryArchives(entriesDirectory);
            WrapLaunchers(entriesDirectory);
            
            var foundEntries = SearchEntryDirectories(entriesDirectory);
            return foundEntries.OrderBy(entry => entry.Files.DirectoryPath.Value).ToList();
        }

        private void WrapLaunchers(DirectoryInfo entriesDirectory)
        {
            var windowsExecutables = entriesDirectory.EnumerateFiles("*.exe", SearchOption.TopDirectoryOnly);
            var gxGameFiles = entriesDirectory.EnumerateFiles("*.gxgame", SearchOption.TopDirectoryOnly);

            foreach (var launcherFile in windowsExecutables.Concat(gxGameFiles))
            {
                var launcherPath = FilePath.Of(launcherFile);
                var directoryPath = launcherPath.GetParentDirectoryPath().Append(launcherPath.GetNameWithoutExtension());
                if (Directory.Exists(directoryPath.Value))
                    continue;

                Directory.CreateDirectory(directoryPath.Value);
                var targetPath = directoryPath.Append(launcherFile.Name);
                launcherFile.MoveTo(targetPath.Value);
            }
        }

        private void ExtractEntryArchives(DirectoryInfo entriesDirectory)
        {
            var archives = entriesDirectory.EnumerateFiles("*.zip", SearchOption.TopDirectoryOnly);
            foreach (var archive in archives)
            {
                var archivePath = FilePath.Of(archive);
                var targetPath = archivePath.GetParentDirectoryPath().Append(archivePath.GetNameWithoutExtension());
                if (Directory.Exists(targetPath.Value))
                    continue;

                EntryExtractor.ExtractDirectory(archivePath, targetPath);
            }
        }

        private IReadOnlyCollection<JamEntryEditable> SearchEntryDirectories(DirectoryInfo entriesDirectory)
        {
            var result = new List<JamEntryEditable>();
            var entryDirectories = entriesDirectory.EnumerateDirectories();
            foreach (var directory in entryDirectories)
            {
                var directoryPath = FilePath.Of(directory);
                var entryEditable = EntryExplorer.LoadFromDirectory(directoryPath);
                var saveModel = new JamEntrySaveModel(isFromEntryPackager: false);
                saveModel.Save(entryEditable);
                result.Add(entryEditable);
            }
            return result;
        }
    }
}
