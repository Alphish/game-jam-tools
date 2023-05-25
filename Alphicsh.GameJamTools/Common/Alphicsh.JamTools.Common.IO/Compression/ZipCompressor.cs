using System.IO;
using System.IO.Compression;

namespace Alphicsh.JamTools.Common.IO.Compression
{
    public class ZipCompressor
    {
        public void CompressDirectoryContentsTo(FilePath zipPath, FilePath compressedDirectoryPath)
        {
            using var zipStream = new FileStream(zipPath.Value, FileMode.Create);
            using var archive = new ZipArchive(zipStream, ZipArchiveMode.Create);

            AddEntriesFromDirectory(archive, compressedDirectoryPath, compressedDirectoryPath.GetDirectory());
        }

        private void AddEntriesFromDirectory(ZipArchive archive, FilePath compressedDirectoryPath, DirectoryInfo directoryInfo)
        {
            foreach (var subdirectory in directoryInfo.GetDirectories())
            {
                AddEntriesFromDirectory(archive, compressedDirectoryPath, subdirectory);
            }
            foreach (var file in directoryInfo.GetFiles())
            {
                var filePath = FilePath.From(file.FullName);
                var entryName = filePath.AsRelativeTo(compressedDirectoryPath).Value;
                archive.CreateEntryFromFile(file.FullName, entryName);
            }
        }
    }
}
