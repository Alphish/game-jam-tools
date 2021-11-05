using Alphicsh.JamTools.Common.Mvvm;

using Alphicsh.JamPlayer.Model.Jam;

namespace Alphicsh.JamPlayer.ViewModel.Jam
{
    public sealed class JamOverviewViewModel : WrapperViewModel<JamOverview>
    {
        public JamOverviewViewModel(JamOverview model)
            : base(model)
        {
            Entries = CollectionViewModel.CreateImmutable(model.Entries, JamEntryViewModel.CollectionStub);
        }

        public CollectionViewModel<JamEntry, JamEntryViewModel> Entries { get; }
    }
}
