using System.Linq;

using Alphicsh.JamPlayer.Model.Jam;

namespace Alphicsh.JamPlayer.ViewModel.Jam
{
    public sealed class JamTeamViewModel : BaseViewModel<JamTeam>
    {
        public JamTeamViewModel(JamTeam model)
            : base(model)
        {
            Authors = new CollectionViewModel<JamAuthor, JamAuthorViewModel>(model.Authors, item => new JamAuthorViewModel(item));
            Description = Model.Name ?? string.Join(", ", Authors.Select(author => author.Name));
        }

        public string? Name => Model.Name;
        public CollectionViewModel<JamAuthor, JamAuthorViewModel> Authors { get; }
        public string Description { get; }
    }
}
