using System.Windows.Input;
using Alphicsh.EntryPackager.Model;
using Alphicsh.EntryPackager.ViewModel.Entry;
using Alphicsh.EntryPackager.ViewModel.Entry.Saving;
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
            OpenEntryZipCommand = SimpleCommand.From(OpenEntryZip);

            SaveSystem = new JamEntrySaveViewModel();
        }

        public JamEntryEditableViewModel? Entry { get; private set; }

        public WrapperProperty<EntryPackagerViewModel, bool> HasEntryProperty { get; }
        public bool HasEntry { get => HasEntryProperty.Value; }

        public JamEntrySaveViewModel SaveSystem { get; }

        // -------
        // Loading
        // -------

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

        public ICommand OpenEntryZipCommand { get; }
        private void OpenEntryZip()
        {
            var zipPath = FileQuery.OpenFile()
                .WithFileType("*.zip", "ZIP archive")
                .GetPath();

            if (zipPath == null)
                return;

            LoadEntryZip(zipPath.Value);
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

        public void LoadEntryZip(FilePath zipPath)
        {
            Model.LoadEntryZip(zipPath);
            UpdateEntryViewModel();
        }

        private void UpdateEntryViewModel()
        {
            Entry = new JamEntryEditableViewModel(Model.Entry!);
            RaisePropertyChanged(nameof(Entry), nameof(HasEntry));
            SaveSystem.TrackViewModel(Entry);
        }
    }
}
