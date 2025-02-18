using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Trophies.Data
{
    public class JamTrophyEntry
    {
        public JamEntry Entry { get; }

        public JamTrophyEntry(JamEntry entry)
        {
            Entry = entry;
            Id = string.Empty;
            Title = entry.FullTitle;
            Authors = entry.Team;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
    }
}
