using Alphicsh.JamTools.Common.Mvvm;

using Alphicsh.JamPlayer.Model.Jam;

namespace Alphicsh.JamPlayer.ViewModel.Jam
{
    public sealed class JamEntryViewModel : BaseViewModel<JamEntry>
    {
        public static CollectionViewModelStub<JamEntry, JamEntryViewModel> CollectionStub { get; }
            = CollectionViewModelStub.Create((JamEntry model) => new JamEntryViewModel(model));

        public JamEntryViewModel(JamEntry model)
            : base(model)
        {
            Team = new JamTeamViewModel(model.Team);
        }

        public string Title => Model.Title;
        public JamTeamViewModel Team { get; }
    }
}
