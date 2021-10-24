using Alphicsh.JamPlayer.Model.Jam;

namespace Alphicsh.JamPlayer.ViewModel.Jam
{
    public sealed class JamAuthorViewModel : BaseViewModel<JamAuthor>
    {
        public JamAuthorViewModel(JamAuthor model)
            : base(model) { }

        public string Name => Model.Name;
    }
}
