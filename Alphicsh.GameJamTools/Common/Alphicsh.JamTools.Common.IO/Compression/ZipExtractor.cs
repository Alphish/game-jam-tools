using System.IO;
using System.IO.Compression;

namespace Alphicsh.JamTools.Common.IO.Compression
{
    public class ZipExtractor
    {
        public bool ExtractDirectory(FilePath zipPath, FilePath targetPath)
        {
            if (!zipPath.HasFile())
                return false;

            using var zipStream = new FileStream(zipPath.Value, FileMode.Open);
            using var archive = OpenArchiveToExtract(zipStream);
            if (archive == null || archive.Entries.Count == 0)
                return false;

            if (!Directory.Exists(targetPath.Value))
                Directory.CreateDirectory(targetPath.Value);

            ExtractArchive(archive, targetPath);
            return true;
        }

        public FilePath? ExtractNewDirectory(FilePath zipPath)
        {
            var targetPath = FindNewDirectoryName(zipPath);
            var extractionResult = ExtractDirectory(zipPath, targetPath);
            return extractionResult ? targetPath : null;
        }

        private ZipArchive? OpenArchiveToExtract(FileStream stream)
        {
            try
            {
                return new ZipArchive(stream, ZipArchiveMode.Read);
            }
            catch (InvalidDataException)
            {
                return null;
            }
        }

        private FilePath FindNewDirectoryName(FilePath zipPath)
        {
            var parentDirectoryPath = zipPath.GetParentDirectoryPath();
            var directoryName = zipPath.GetNameWithoutExtension();
            var directoryPath = parentDirectoryPath.Append(directoryName);
            var repeatIndex = 0;
            while (directoryPath.HasDirectory())
            {
                repeatIndex++;
                directoryPath = parentDirectoryPath.Append($"{directoryName} ({repeatIndex})");
            }

            return directoryPath;
        }

        private void ExtractArchive(ZipArchive archive, FilePath zipTargetPath)
        {
            // if the ZIP includes a top-level directory containing all subsequent entries
            // the extra directory is removed from the extracted content
            var zipCommonDirectory = FindZipCommonDirectory(archive);
            foreach (var entry in archive.Entries)
            {
                ExtractEntry(entry, zipTargetPath, zipCommonDirectory);
            }
        }

        private string FindZipCommonDirectory(ZipArchive archive)
        {
            var firstEntryName = archive.Entries[0].FullName;
            var topDirectoryLength = firstEntryName.IndexOf('/') + 1;
            if (topDirectoryLength <= 0)
                return string.Empty;

            var commonDirectoryCandidate = firstEntryName.Substring(0, topDirectoryLength);
            foreach (var entry in archive.Entries)
            {
                if (!entry.FullName.StartsWith(commonDirectoryCandidate))
                    return string.Empty;
            }

            return commonDirectoryCandidate;
        }

        private void ExtractEntry(ZipArchiveEntry entry, FilePath zipTargetPath, string zipCommonDirectory)
        {
            var relativePath = entry.FullName.Substring(zipCommonDirectory.Length);
            var extractionPath = zipTargetPath.Append(relativePath);

            if (entry.Name == "") // entry is directory
            {
                Directory.CreateDirectory(extractionPath.Value);
            }
            else
            {
                var parentDirectoryPath = extractionPath.GetParentDirectoryPath();
                Directory.CreateDirectory(parentDirectoryPath.Value);
                entry.ExtractToFile(extractionPath.Value, overwrite: true);
            }
        }
    }
}
