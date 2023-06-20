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
            return new JamOverview
            {
                AwardCriteria = jamInfo.AwardCriteria.Select(MapAwardCriterion).ToList(),
                Entries = MapEntries(entriesPath, jamInfo.Entries),
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

        private IReadOnlyCollection<JamEntry> MapEntries(FilePath entriesPath, IEnumerable<JamEntryStub> stubs)
        {
            var result = new List<JamEntry>();
            foreach (var stub in stubs)
            {
                var entry = TryLoadEntry(entriesPath, stub);
                if (entry != null)
                    result.Add(entry);
            }
            return result;
        }

        private JamEntry? TryLoadEntry(FilePath entriesPath, JamEntryStub stub)
        {
            var id = stub.Id;
            var directoryPath = entriesPath.Append(stub.EntrySubpath);
            return EntryLoader.ReadFromDirectory(id, directoryPath);
        }
    }
}
