using System.Collections.Generic;
using System.Linq;

using Alphicsh.JamPlayer.Model.Awards;
using Alphicsh.JamPlayer.ViewModel.Jam;
using Alphicsh.JamTools.Common.Mvvm;

namespace Alphicsh.JamPlayer.ViewModel.Awards
{
    public class AwardsOverviewViewModel : WrapperViewModel<AwardsOverview>
    {
        public AwardsOverviewViewModel(AwardsOverview model, JamOverviewViewModel jamOverview) : base(model)
        {
            Entries = model.Entries.Select(entry => new AwardEntryViewModel(entry, jamOverview)).ToList();
        }

        public IReadOnlyCollection<AwardEntryViewModel> Entries { get; set; }
    }
}
