using Alphicsh.JamPlayer.Model.Jam;

namespace Alphicsh.JamPlayer.ViewModel.Jam
{
    public sealed class JamEntryViewModel : BaseViewModel<JamEntry>
    {
        public JamEntryViewModel(JamEntry model)
            : base(model)
        {
            Team = new JamTeamViewModel(model.Team);
        }

        public string Title => Model.Title;
        public JamTeamViewModel Team { get; }
    }
}
