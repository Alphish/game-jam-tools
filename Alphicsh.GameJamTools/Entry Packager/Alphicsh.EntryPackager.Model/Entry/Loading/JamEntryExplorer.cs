using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Compression;

namespace Alphicsh.EntryPackager.Model.Entry.Loading
{
    public class JamEntryExplorer
    {
        private ZipExtractor Extractor { get; } = new ZipExtractor();

        private JamEntryKnownDataReader KnownDataReader { get; } = new JamEntryKnownDataReader();
        private JamEntryUnknownDataFinder UnknownDataFinder { get; } = new JamEntryUnknownDataFinder();

        public JamEntryEditable LoadFromDirectory(FilePath directoryPath)
        {
            return KnownDataReader.TryReadFromDirectory(directoryPath)
                ?? UnknownDataFinder.FindEntryData(directoryPath);
        }

        public JamEntryEditable LoadFromFile(FilePath entryInfoPath)
        {
            return KnownDataReader.TryReadFromFile(entryInfoPath)
                ?? UnknownDataFinder.FindEntryData(entryInfoPath.GetParentDirectoryPath()!.Value);
        }

        public JamEntryEditable LoadFromZip(FilePath zipPath)
        {
            var directoryPath = Extractor.ExtractNewDirectoryFrom(zipPath)!.Value;
            var directoryName = zipPath.GetNameWithoutExtension();

            return KnownDataReader.TryReadFromDirectory(directoryPath)
                ?? UnknownDataFinder.FindEntryData(directoryPath, directoryName);
        }
    }
}
