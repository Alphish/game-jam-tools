using System.Linq;

using Alphicsh.JamTools.Common.Mvvm;

using Alphicsh.JamPlayer.Model.Jam;
using System.Collections.Generic;

namespace Alphicsh.JamPlayer.ViewModel.Jam
{
    public sealed class JamTeamViewModel : BaseViewModel<JamTeam>
    {
        public JamTeamViewModel(JamTeam model)
            : base(model)
        {
            Authors = CollectionViewModel.CreateImmutable(model.Authors, JamAuthorViewModel.CollectionStub);
            ShortDescription = Model.Name ?? string.Join(", ", Authors.Select(author => author.Name));

            DescriptionItems = BuildDescriptionItems();
        }

        public string? Name => Model.Name;
        public CollectionViewModel<JamAuthor, JamAuthorViewModel> Authors { get; }
        public string ShortDescription { get; }

        // -----------------
        // Description items
        // -----------------

        public IReadOnlyCollection<object> DescriptionItems { get; }

        private IReadOnlyCollection<object> BuildDescriptionItems()
        {
            var displayItems = new List<object>();

            foreach (var author in Authors)
            {
                displayItems.Add(author);
                if (author != Authors.Last())
                    displayItems.Add(", ");
            }
            if (Name != null)
            {
                displayItems.Insert(0, this);
                displayItems.Insert(1, " (");
                displayItems.Add(")");
            }

            return displayItems;
        }
    }
}
