using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Jam.New;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.JamTally.Model.Jam.Loading
{
    public class JamLoader
    {
        private static JsonFileLoader<NewJamCore> InfoLoader { get; } = new JsonFileLoader<NewJamCore>();
        private static JamEntryLoader EntryLoader { get; } = new JamEntryLoader();

        private static JsonFileLoader<JamOverrides> OverridesLoader { get; } = new JsonFileLoader<JamOverrides>();
        private static JsonFileSaver<JamOverrides> OverridesSaver { get; } = new JsonFileSaver<JamOverrides>();

        public JamOverview? LoadFromDirectory(FilePath directoryPath)
        {
            var jamInfoPath = directoryPath.Append("jam.jaminfo");
            var jamInfo = InfoLoader.TryLoad(jamInfoPath);

            var jamOverridesPath = directoryPath.Append(".jamtally/overrides.jamoverrides");
            var loadedOverrides = OverridesLoader.TryLoad(jamOverridesPath);
            var jamOverrides = loadedOverrides ?? new JamOverrides { Entries = new List<JamEntryOverride>() };

            var result = jamInfo != null ? MapJam(directoryPath, jamInfo, jamOverrides) : null;
            if (loadedOverrides == null && result != null)
                GenerateOverrides(jamOverridesPath, result);

            return result;
        }

        // -------
        // Mapping
        // -------

        private JamOverview MapJam(FilePath directoryPath, NewJamCore jamInfo, JamOverrides overrides)
        {
            var entriesPath = directoryPath.Append(jamInfo.EntriesSubpath);
            var awardCriteria = jamInfo.AwardCriteria
                .Select(criterion => MapAwardCriterion(criterion, overrides.GetAward(criterion.Id)))
                .ToList();

            var alignments = MapAlignments(jamInfo.Alignments);
            var jam = new JamOverview
            {
                DirectoryPath = directoryPath,
                AwardCriteria = awardCriteria,
                Alignments = alignments,
                Entries = MapEntries(entriesPath, jamInfo.Entries, alignments, overrides),
            };
            jam.Search = new Vote.Search.JamSearch(jam);
            return jam;
        }

        private JamAwardCriterion MapAwardCriterion(NewJamAwardInfo awardInfo, JamAwardOverride? awardOverride)
        {
            var defaultAbbreviation = awardInfo.Id.Length > 3
                ? awardInfo.Id.Remove(3).ToUpperInvariant()
                : awardInfo.Id.ToUpperInvariant();

            return new JamAwardCriterion
            {
                Id = awardInfo.Id,
                Name = awardInfo.FixedName,
                TallyName = awardOverride?.Name ?? awardInfo.FixedName,
                Abbreviation = awardOverride?.Abbreviation ?? defaultAbbreviation,
            };
        }

        private JamAlignments? MapAlignments(NewJamAlignmentInfo? alignments)
        {
            if (alignments == null)
                return null;

            var neitherTitle = alignments.NeitherTitle;
            var options = alignments.Options.Select(MapAlignmentOption);
            return new JamAlignments(neitherTitle, options);
        }

        private JamAlignmentOption MapAlignmentOption(NewJamAlignmentOptionInfo optionInfo)
        {
            return new JamAlignmentOption
            {
                Title = optionInfo.Title,
                ShortTitle = optionInfo.ShortTitle,
            };
        }

        private IReadOnlyCollection<JamEntry> MapEntries(FilePath entriesPath, IEnumerable<NewJamEntryStub> stubs, JamAlignments? alignments, JamOverrides overrides)
        {
            var result = new List<JamEntry>();
            foreach (var stub in stubs)
            {
                var entry = TryLoadEntry(entriesPath, stub, alignments, overrides.GetEntry(stub.Id));
                if (entry != null)
                    result.Add(entry);
            }
            return result;
        }

        private JamEntry? TryLoadEntry(FilePath entriesPath, NewJamEntryStub stub, JamAlignments? alignments, JamEntryOverride? entryOverride)
        {
            var id = stub.Id;
            var directoryPath = entriesPath.Append(stub.EntrySubpath);
            return EntryLoader.ReadFromDirectory(id, directoryPath, alignments, entryOverride);
        }

        // ---------
        // Overrides
        // ---------

        private void GenerateOverrides(FilePath overridesPath, JamOverview jam)
        {
            var awardOverrides = jam.AwardCriteria.Select(criterion => new JamAwardOverride
            {
                Id = criterion.Id,
                Name = criterion.TallyName,
                Abbreviation = criterion.Abbreviation,
            }).ToList();

            var entryOverrides = jam.Entries.Select(entry => new JamEntryOverride
            {
                EntryId = entry.Id,
                TallyCode = entry.TallyCode,
                TallyTitle = entry.TallyTitle,
                TallyAuthors = entry.TallyAuthors,
            }).ToList();

            var jamOverrides = new JamOverrides { Awards = awardOverrides, Entries = entryOverrides };

            OverridesSaver.Save(overridesPath, jamOverrides);
        }
    }
}
