using System.Collections.Generic;
using System.Linq;

using Alphicsh.JamPlayer.Model.Awards;
using Alphicsh.JamPlayer.ViewModel.Jam;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPlayer.ViewModel.Awards
{
    public class AwardEntryViewModel : WrapperViewModel<AwardEntry>
    {
        public AwardEntryViewModel(AwardEntry model, JamOverviewViewModel jamOverview) : base(model)
        {
            Candidates = ListCandidates(jamOverview);

            var currentCandidate = Candidates.Single(candidate => candidate.Entry?.Model.Id == model.JamEntry?.Id);
            EntryProperty = MutableProperty.Create(this, nameof(Entry), currentCandidate);
        }

        public string Name => Model.Criterion.Name;
        public string Description => Model.Criterion.Description;

        public MutableProperty<AwardCandidateViewModel> EntryProperty { get; }
        public AwardCandidateViewModel Entry
        {
            get => EntryProperty.Value;
            set
            {
                Model.JamEntry = value.Entry?.Model;
                EntryProperty.Value = value;
            }
        }

        public IReadOnlyCollection<AwardCandidateViewModel> Candidates { get; set; }

        private IReadOnlyCollection<AwardCandidateViewModel> ListCandidates(JamOverviewViewModel jamOverview)
        {
            var candidates = jamOverview.Entries
                .OrderBy(entry => entry.Title)
                .ThenBy(entry => entry.Team.ShortDescription)
                .Select(entry => new AwardCandidateViewModel { Entry = entry })
                .ToList();
            candidates.Insert(0, AwardCandidateViewModel.Empty);

            return candidates;
        }
    }
}
