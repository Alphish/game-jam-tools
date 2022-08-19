using Alphicsh.JamPlayer.ViewModel.Jam;
using Alphicsh.JamTools.Common.Mvvm;

namespace Alphicsh.JamPlayer.ViewModel.Awards
{
    public class AwardCandidateViewModel : BaseViewModel
    {
        public static AwardCandidateViewModel Empty { get; }
            = new AwardCandidateViewModel { Entry = null };

        public JamEntryViewModel? Entry { get; init; }
        public string EntryTitle => Entry?.Title ?? "<no entry selected>";
        public string EntryBy => Entry != null ? "by " + Entry.Team.ShortDescription : string.Empty;
    }
}
