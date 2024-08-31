using System.Collections.Generic;
using System.Linq;

using Alphicsh.JamPlayer.Model.Awards;
using Alphicsh.JamPlayer.ViewModel.Jam;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPlayer.ViewModel.Awards
{
    public class AwardsOverviewViewModel : WrapperViewModel<AwardsOverview>
    {
        public AwardsOverviewViewModel(AwardsOverview model, JamOverviewViewModel jamOverview) : base(model)
        {
            Entries = model.Entries.Select(entry => new AwardEntryViewModel(entry, jamOverview)).ToList();

            BestReviewerProperty = WrapperProperty.ForMember(this, vm => vm.Model.BestReviewer);
        }

        public IReadOnlyCollection<AwardEntryViewModel> Entries { get; set; }

        public WrapperProperty<AwardsOverviewViewModel, string?> BestReviewerProperty { get; }
        public string? BestReviewer { get => BestReviewerProperty.Value; set => BestReviewerProperty.Value = value; }
    }
}
