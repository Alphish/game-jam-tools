using System.Linq;
using Alphicsh.JamTools.Common.IO.Jam;
using Alphicsh.JamTools.Common.IO.Serialization;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTally.Model.Jam.Loading
{
    public class JamEntryLoader
    {
        private static JsonFileLoader<JamEntryInfo> Loader { get; } = new JsonFileLoader<JamEntryInfo>();

        public JamEntry? ReadFromDirectory(string id, FilePath directoryPath, JamAlignments? alignments)
        {
            var entryInfoPath = directoryPath.Append("entry.jamentry");
            var entryInfo = Loader.TryLoad(entryInfoPath)?.FromLegacyFormat();
            return entryInfo != null ? MapEntry(id, entryInfo, alignments) : null;
        }

        // -------
        // Mapping
        // -------

        private JamEntry MapEntry(string id, JamEntryInfo entryInfo, JamAlignments? alignments)
        {
            var authorNames = entryInfo.Team.Authors.Select(author => author.Name).ToList();
            return new JamEntry
            {
                Id = id,
                Title = entryInfo.ShortTitle ?? entryInfo.Title,
                FullTitle = entryInfo.Title,
                Team = entryInfo.Team.Name ?? string.Join(", ", authorNames),
                RawTeam = entryInfo.Team.Name,
                Authors = authorNames,
                Alignment = alignments?.GetAlignment(entryInfo.Alignment),
            };
        }
    }
}
