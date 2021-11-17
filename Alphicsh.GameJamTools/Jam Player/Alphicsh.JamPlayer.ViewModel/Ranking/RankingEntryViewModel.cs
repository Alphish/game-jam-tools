using System.Collections.Generic;
using System.Linq;

using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

using Alphicsh.JamPlayer.Model.Ranking;
using Alphicsh.JamPlayer.ViewModel.Jam;
using Alphicsh.JamPlayer.ViewModel.Ratings;

namespace Alphicsh.JamPlayer.ViewModel.Ranking
{
    public sealed class RankingEntryViewModel : WrapperViewModel<RankingEntry>
    {
        public static CollectionViewModelStub<RankingEntry, RankingEntryViewModel> CollectionStub { get; }
            = CollectionViewModelStub.Create((RankingEntry model) => new RankingEntryViewModel(model));

        public RankingEntryViewModel(RankingEntry model)
            : base(model)
        {
            JamEntry = new JamEntryViewModel(model.JamEntry);

            RankProperty = MutableProperty.Create(this, nameof(Rank), (int?)null)
                .WithDependingProperty(nameof(IsRanked));

            Ratings = model.Ratings.Select(RatingViewModel.Create).ToList();

            CommentProperty = WrapperProperty.Create(this, nameof(Comment), vm => vm.Model.Comment, (vm, value) => vm.Model.Comment = value);
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

        public IReadOnlyCollection<RatingViewModel> Ratings { get; private set; }

        public WrapperProperty<RankingEntryViewModel, string> CommentProperty { get; }
        public string Comment { get => CommentProperty.Value; set => CommentProperty.Value = value; }
    }
}
