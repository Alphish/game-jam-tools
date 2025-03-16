using System.Linq;
using System.Text;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Jam;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.JamTally.Model.Jam.Loading
{
    public class JamEntryLoader
    {
        private static JsonFileLoader<JamEntryInfo> Loader { get; } = new JsonFileLoader<JamEntryInfo>();

        public JamEntry? ReadFromDirectory(string id, FilePath directoryPath, JamAlignments? alignments, JamEntryOverride? entryOverride)
        {
            var entryInfoPath = directoryPath.Append("entry.jamentry");
            var entryInfo = Loader.TryLoad(entryInfoPath)?.FromLegacyFormat();
            return entryInfo != null ? MapEntry(id, entryInfo, alignments, entryOverride) : null;
        }

        // -------
        // Mapping
        // -------

        private JamEntry MapEntry(string id, JamEntryInfo entryInfo, JamAlignments? alignments, JamEntryOverride? entryOverride)
        {
            var authorNames = entryInfo.Team.Authors.Select(author => author.Name).ToList();
            var title = entryInfo.ShortTitle ?? entryInfo.Title;
            var code = ExtractEntryCode(title);
            var team = entryInfo.Team.Name ?? string.Join(", ", authorNames);
            return new JamEntry
            {
                Id = id,
                Title = entryInfo.ShortTitle ?? entryInfo.Title,
                FullTitle = entryInfo.Title,
                Team = entryInfo.Team.Name ?? string.Join(", ", authorNames),
                RawTeam = entryInfo.Team.Name,
                Authors = authorNames,
                Alignment = alignments?.GetAlignment(entryInfo.Alignment),
                TallyCode = entryOverride?.TallyCode ?? code,
                TallyTitle = entryOverride?.TallyTitle ?? title,
                TallyAuthors = entryOverride?.TallyAuthors ?? team,
            };
        }

        private string ExtractEntryCode(string title)
        {
            var codeBuilder = new StringBuilder();
            foreach (char c in title)
            {
                if (c >= 128)
                    continue;

                if (char.IsLetterOrDigit(c))
                    codeBuilder.Append(c);
                else if (codeBuilder.Length > 0 && codeBuilder[^1] != '_')
                    codeBuilder.Append('_');
            }
            return codeBuilder.ToString().ToLowerInvariant();
        }
    }
}
