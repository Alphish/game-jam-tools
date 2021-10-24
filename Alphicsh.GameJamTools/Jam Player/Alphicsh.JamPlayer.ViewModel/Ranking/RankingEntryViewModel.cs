using Alphicsh.JamPlayer.Model.Ranking;
using Alphicsh.JamPlayer.ViewModel.Jam;

namespace Alphicsh.JamPlayer.ViewModel.Ranking
{
    public sealed class RankingEntryViewModel : BaseViewModel<RankingEntry>
    {
        public RankingEntryViewModel(RankingEntry model)
            : base(model)
        {
            JamEntry = new JamEntryViewModel(model.JamEntry);
        }

        public JamEntryViewModel JamEntry { get; }
    }
}
