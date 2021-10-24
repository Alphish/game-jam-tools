using Alphicsh.JamPlayer.Model.Ranking;
using Alphicsh.JamPlayer.ViewModel.Jam;

namespace Alphicsh.JamPlayer.ViewModel.Ranking
{
    public sealed class RankingEntryViewModel : BaseViewModel<RankingEntry>
    {
        public static CollectionViewModelStub<RankingEntry, RankingEntryViewModel> CollectionStub { get; }
            = CollectionViewModelStub.Create((RankingEntry model) => new RankingEntryViewModel(model));

        public RankingEntryViewModel(RankingEntry model)
            : base(model)
        {
            JamEntry = new JamEntryViewModel(model.JamEntry);
            RankProperty = new MutableProperty<int?>(
                this, nameof(Rank), null,
                dependingProperties: new string[] { nameof(IsRanked) }
                );
        }

        // ---------------
        // Read properties
        // ---------------

        public JamEntryViewModel JamEntry { get; }

        // ------------------
        // Mutable properties
        // ------------------

        private MutableProperty<int?> RankProperty { get; }
        public int? Rank { get => RankProperty.Value; set => RankProperty.Value = value; }
        public bool IsRanked => Rank.HasValue;
    }
}
