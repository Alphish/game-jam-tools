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
            OpenEntryInfoCommand = SimpleCommand.From(OpenEntryInfo);
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

        public ICommand OpenEntryInfoCommand { get; }
        private void OpenEntryInfo()
        {
            var entryInfoPath = FileQuery.OpenFile()
                .WithFileType("entry.jamentry", "Jam entry data")
                .GetPath();

            if (entryInfoPath == null)
                return;

            LoadEntryInfo(entryInfoPath.Value);
        }

        public void LoadEntryDirectory(FilePath directoryPath)
        {
            Model.LoadDirectory(directoryPath);
            UpdateEntryViewModel();
        }

        public void LoadEntryInfo(FilePath entryInfoPath)
        {
            Model.LoadEntryInfo(entryInfoPath);
            UpdateEntryViewModel();
        }

        private void UpdateEntryViewModel()
        {
            Entry = new JamEntryEditableViewModel(Model.Entry!);
            RaisePropertyChanged(nameof(Entry), nameof(HasEntry));
        }
    }
}
