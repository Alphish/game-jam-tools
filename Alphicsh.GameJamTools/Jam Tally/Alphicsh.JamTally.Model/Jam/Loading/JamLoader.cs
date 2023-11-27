using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Jam;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.JamTally.Model.Jam.Loading
{
    public class JamLoader
    {
        private static JsonFileLoader<JamInfo> InfoLoader { get; } = new JsonFileLoader<JamInfo>();
        private static JamEntryLoader EntryLoader { get; } = new JamEntryLoader();

        public JamOverview? LoadFromDirectory(FilePath directoryPath)
        {
            var jamInfoPath = directoryPath.Append("jam.jaminfo");
            var jamInfo = InfoLoader.TryLoad(jamInfoPath);
            return jamInfo != null ? MapJam(directoryPath, jamInfo) : null;
        }

        // -------
        // Mapping
        // -------

        private JamOverview MapJam(FilePath directoryPath, JamInfo jamInfo)
        {
            var entriesPath = directoryPath.Append(jamInfo.EntriesSubpath);
            var alignments = MapAlignments(jamInfo.Alignments);
            return new JamOverview
            {
                AwardCriteria = jamInfo.AwardCriteria.Select(MapAwardCriterion).ToList(),
                Alignments = alignments,
                Entries = MapEntries(entriesPath, jamInfo.Entries, alignments),
            };
        }

        private JamAwardCriterion MapAwardCriterion(JamAwardInfo awardInfo)
        {
            return new JamAwardCriterion
            {
                Id = awardInfo.Id,
                Name = awardInfo.FixedName,
            };
        }

        private JamAlignments? MapAlignments(JamAlignmentInfo? alignments)
        {
            if (alignments == null)
                return null;

            var neitherTitle = alignments.NeitherTitle;
            var options = alignments.Options.Select(MapAlignmentOption);
            return new JamAlignments(neitherTitle, options);
        }

        private JamAlignmentOption MapAlignmentOption(JamAlignmentOptionInfo optionInfo)
        {
            return new JamAlignmentOption
            {
                Title = optionInfo.Title,
                ShortTitle = optionInfo.ShortTitle,
            };
        }

        private IReadOnlyCollection<JamEntry> MapEntries(FilePath entriesPath, IEnumerable<JamEntryStub> stubs, JamAlignments? alignments)
        {
            var result = new List<JamEntry>();
            foreach (var stub in stubs)
            {
                var entry = TryLoadEntry(entriesPath, stub, alignments);
                if (entry != null)
                    result.Add(entry);
            }
            return result;
        }

        private JamEntry? TryLoadEntry(FilePath entriesPath, JamEntryStub stub, JamAlignments? alignments)
        {
            var id = stub.Id;
            var directoryPath = entriesPath.Append(stub.EntrySubpath);
            return EntryLoader.ReadFromDirectory(id, directoryPath, alignments);
        }
    }
}
