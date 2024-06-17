using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.ViewModel.Jam
{
    public class JamEntryOptionViewModel
    {
        public JamEntryOptionViewModel(JamEntry? entry, string description)
        {
            Entry = entry;
            Description = description;
        }

        public JamEntry? Entry { get; }
        public string Description { get; }
    }
}
