using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.EntryPackager.Model.Entry.Exploration
{
    public class JamEntryExplorer
    {
        private JamEntryKnownDataReader KnownDataReader { get; } = new JamEntryKnownDataReader();
        private JamEntryUnknownDataFinder UnknownDataFinder { get; } = new JamEntryUnknownDataFinder();

        public JamEntryEditable ReadFromDirectory(FilePath directoryPath)
        {
            return KnownDataReader.TryReadFromDirectory(directoryPath)
                ?? UnknownDataFinder.FindEntryData(directoryPath);
        }

        public JamEntryEditable ReadFromFile(FilePath entryInfoPath)
        {
            return KnownDataReader.TryReadFromFile(entryInfoPath)
                ?? UnknownDataFinder.FindEntryData(entryInfoPath.GetParentDirectoryPath()!.Value);
        }
    }
}
