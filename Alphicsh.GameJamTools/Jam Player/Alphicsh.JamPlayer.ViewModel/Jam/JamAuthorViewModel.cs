﻿using Alphicsh.JamPlayer.Model.Jam;

namespace Alphicsh.JamPlayer.ViewModel.Jam
{
    public sealed class JamAuthorViewModel : BaseViewModel<JamAuthor>
    {
        public static CollectionViewModelStub<JamAuthor, JamAuthorViewModel> CollectionStub { get; }
            = CollectionViewModelStub.Create((JamAuthor model) => new JamAuthorViewModel(model));

        public JamAuthorViewModel(JamAuthor model)
            : base(model) { }

        public string Name => Model.Name;
    }
}
