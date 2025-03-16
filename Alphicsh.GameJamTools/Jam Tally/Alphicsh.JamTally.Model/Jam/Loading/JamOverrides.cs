using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamTally.Model.Jam.Loading
{
    public class JamOverrides
    {
        private IReadOnlyCollection<JamAwardOverride> AwardOverrides { get; set; } = new List<JamAwardOverride>();
        private IReadOnlyDictionary<string, JamAwardOverride> AwardOverridesById { get; set; } = new Dictionary<string, JamAwardOverride>();
        private IReadOnlyCollection<JamEntryOverride> EntryOverrides { get; set; } = new List<JamEntryOverride>();
        private IReadOnlyDictionary<string, JamEntryOverride> EntryOverridesById { get; set; } = new Dictionary<string, JamEntryOverride>();

        public IReadOnlyCollection<JamAwardOverride> Awards
        {
            get => AwardOverrides;
            init
            {
                AwardOverrides = value;
                AwardOverridesById = AwardOverrides.ToDictionary(awardOverride => awardOverride.Id);
            }
        }

        public IReadOnlyCollection<JamEntryOverride> Entries
        {
            get => EntryOverrides;
            init
            {
                EntryOverrides = value;
                EntryOverridesById = EntryOverrides.ToDictionary(entryOverride => entryOverride.EntryId);
            }
        }

        public JamAwardOverride? GetAward(string id)
            => AwardOverridesById.TryGetValue(id, out var awardOverride) ? awardOverride : null;

        public JamEntryOverride? GetEntry(string id)
            => EntryOverridesById.TryGetValue(id, out var entryOverride) ? entryOverride : null;
    }
}
