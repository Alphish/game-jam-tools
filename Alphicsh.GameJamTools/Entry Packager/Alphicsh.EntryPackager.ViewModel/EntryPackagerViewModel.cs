using System.Windows.Input;
using Alphicsh.EntryPackager.Model;
using Alphicsh.EntryPackager.ViewModel.Entry;
using Alphicsh.JamTools.Common.Controls.Files;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.EntryPackager.ViewModel
{
    public class EntryPackagerViewModel : AppViewModel<AppModel>
    {
        public EntryPackagerViewModel(AppModel model) : base(model)
        {
            Entry = null;
            HasEntryProperty = WrapperProperty.ForReadonlyMember(this, vm => vm.Model.HasEntry);

            OpenEntryDirectoryCommand = SimpleCommand.From(OpenEntryDirectory);
        }

        public JamEntryEditableViewModel? Entry { get; private set; }

        public WrapperProperty<EntryPackagerViewModel, bool> HasEntryProperty { get; }
        public bool HasEntry { get => HasEntryProperty.Value; }

        public ICommand OpenEntryDirectoryCommand { get; }
        private void OpenEntryDirectory()
        {
            var directoryPath = FileQuery.OpenDirectory().GetPath();
            if (directoryPath == null)
                return;

            LoadEntryDirectory(directoryPath.Value);
        }

        public void LoadEntryDirectory(FilePath directoryPath)
        {
            Model.LoadDirectory(directoryPath);
            Entry = new JamEntryEditableViewModel(Model.Entry!);
            RaisePropertyChanged(nameof(Entry), nameof(HasEntry));
        }
    }
}
