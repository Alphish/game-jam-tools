using System.IO;
using System.IO.Compression;

namespace Alphicsh.JamTools.Common.IO.Compression
{
    public class ZipExtractor
    {
        public FilePath? ExtractNewDirectoryFrom(FilePath zipPath)
        {
            if (!zipPath.HasFile())
                return null;

            using var zipStream = new FileStream(zipPath.Value, FileMode.Open);
            using var archive = OpenArchiveToExtract(zipStream);
            if (archive == null || archive.Entries.Count == 0)
                return null;

            var extractionDirectoryPath = CreateExtractionDirectory(zipPath);
            ExtractArchive(archive, extractionDirectoryPath);

            return extractionDirectoryPath;
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

        private FilePath CreateExtractionDirectory(FilePath zipPath)
        {
            var parentDirectoryPath = zipPath.GetParentDirectoryPath()!.Value;
            var directoryName = zipPath.GetNameWithoutExtension();
            var directoryPath = parentDirectoryPath.Append(directoryName);
            var repeatIndex = 0;
            while (directoryPath.HasDirectory())
            {
                repeatIndex++;
                directoryPath = parentDirectoryPath.Append($"{directoryName} ({repeatIndex})");
            }

            Directory.CreateDirectory(directoryPath.Value);

            return directoryPath;
        }

        private void ExtractArchive(ZipArchive archive, FilePath extractionDirectoryPath)
        {
            // if the ZIP includes a top-level directory containing all subsequent entries
            // the extra directory is removed from the extracted content
            var zipCommonDirectory = FindZipCommonDirectory(archive);
            foreach (var entry in archive.Entries)
            {
                ExtractEntry(entry, extractionDirectoryPath, zipCommonDirectory);
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

        private void ExtractEntry(ZipArchiveEntry entry, FilePath extractionDirectoryPath, string zipCommonDirectory)
        {
            var relativePath = entry.FullName.Substring(zipCommonDirectory.Length);
            var extractionPath = extractionDirectoryPath.Append(relativePath);

            if (entry.Name == "") // entry is directory
            {
                Directory.CreateDirectory(extractionPath.Value);
            }
            else
            {
                var parentDirectoryPath = extractionPath.GetParentDirectoryPath()!.Value;
                Directory.CreateDirectory(parentDirectoryPath.Value);
                entry.ExtractToFile(extractionPath.Value, overwrite: true);
            }
        }
    }
}
