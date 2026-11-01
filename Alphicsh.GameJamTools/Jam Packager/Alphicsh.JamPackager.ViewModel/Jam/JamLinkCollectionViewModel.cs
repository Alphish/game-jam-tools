using System;
using System.Collections.Generic;
using System.Windows.Input;
using Alphicsh.JamPackager.Model.Jam;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.JamPackager.ViewModel.Jam
{
    public class JamLinkCollectionViewModel : CollectionViewModel<JamLinkEditable, JamLinkEditableViewModel>
    {
        public static CollectionViewModelStub<JamLinkEditable, JamLinkEditableViewModel> CollectionStub
            => CollectionViewModelStub.Create((JamLinkEditable model) => new JamLinkEditableViewModel(model));

        public JamLinkCollectionViewModel(IList<JamLinkEditable> modelEntries)
            : base(modelEntries, CollectionStub, isImmutable: false)
        {
            AddLinkCommand = SimpleCommand.From(AddLink);
            RemoveLinkCommand = SimpleCommand.WithParameter<JamLinkEditableViewModel>(RemoveLink);
            OpenLinkCommand = SimpleCommand.WithParameter<JamLinkEditableViewModel>(OpenLink);
        }

        public ICommand AddLinkCommand { get; }
        private void AddLink()
        {
            var linkModel = new JamLinkEditable { Title = "Example", Url = "http://example.com/" };
            var linkVm = new JamLinkEditableViewModel(linkModel);
            Add(linkVm);
            CompleteChanges();
        }

        public ICommand RemoveLinkCommand { get; }
        private void RemoveLink(JamLinkEditableViewModel linkVm)
        {
            Remove(linkVm);
            CompleteChanges();
        }

        private static WebsiteLauncher LinkLauncher { get; } = new WebsiteLauncher();

        public ICommand OpenLinkCommand { get; }
        private void OpenLink(JamLinkEditableViewModel linkVm)
        {
            if (!Uri.TryCreate(linkVm.Url, UriKind.Absolute, out var uri))
                return;

            LinkLauncher.LaunchFrom(uri);
        }
    }
}
