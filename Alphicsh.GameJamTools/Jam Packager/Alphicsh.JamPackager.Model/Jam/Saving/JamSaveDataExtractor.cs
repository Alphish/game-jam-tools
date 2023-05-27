using System.Linq;
using Alphicsh.EntryPackager.Model.Entry;
using Alphicsh.EntryPackager.Model.Entry.Saving;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Jam;
using Alphicsh.JamTools.Common.IO.Saving;

namespace Alphicsh.JamPackager.Model.Jam.Saving
{
    public class JamSaveDataExtractor : ISaveDataExtractor<JamEditable, JamSaveData>
    {
        private static JamEntrySaveDataExtractor EntryExtractor { get; } = new JamEntrySaveDataExtractor();

        public JamSaveData ExtractData(JamEditable model)
        {
            return new JamSaveData
            {
                DirectoryPath = model.DirectoryPath,
                JamInfo = MapJam(model),
                EntriesData = model.Entries.Select(EntryExtractor.ExtractData).ToList(),
            };
        }

        // -----------
        // Mapping jam
        // -----------

        private JamInfo MapJam(JamEditable jamEditable)
        {
            var entriesPath = jamEditable.DirectoryPath.Append(jamEditable.EntriesLocation);

            return new JamInfo
            {
                Title = jamEditable.Title,
                Theme = jamEditable.Theme,
                AwardCriteria = jamEditable.Awards.Select(MapAward).ToList(),
                EntriesSubpath = jamEditable.EntriesLocation,
                EntriesStubs = jamEditable.Entries.Select(entry => MapEntryToStub(entriesPath, entry)).ToList(),
            };
        }

        private JamAwardInfo MapAward(JamAwardEditable awardEditable)
        {
            return new JamAwardInfo
            {
                Id = awardEditable.Id,
                Description = awardEditable.Description,
            };
        }

        private JamEntryStub MapEntryToStub(FilePath entriesPath, JamEntryEditable entryEditable)
        {
            return new JamEntryStub
            {
                Id = entryEditable.DisplayShortTitle + " by " + entryEditable.Team.DisplayName,
                EntrySubpath = entryEditable.Files.DirectoryPath.AsRelativeTo(entriesPath).Value,
            };
        }
    }
}
