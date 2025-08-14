using System.Threading.Tasks;
using System.Windows.Input;
using Alphicsh.JamPackager.Model;
using Alphicsh.JamPackager.ViewModel.Jam;
using Alphicsh.JamPackager.ViewModel.Saving;
using Alphicsh.JamTools.Common.Controls.Files;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPackager.ViewModel
{
    public class JamPackagerViewModel : AppViewModel<JamPackagerModel>
    {
        public static JamPackagerViewModel Current => (JamPackagerViewModel)AppViewModel.Current;

        public JamPackagerViewModel(JamPackagerModel model) : base(model)
        {
            HasJamProperty = NotifiableProperty.Create(this, nameof(HasJam));

            OpenJamDirectoryCommand = SimpleCommand.From(OpenJamDirectory);
            CloseJamCommand = SimpleCommand.From(CloseJam);
            ExportCompatibilityCommand = SimpleCommand.From(ExportCompatibility);

            SaveSystem = new JamSaveViewModel();
        }

        public JamEditableViewModel? Jam { get; private set; }

        public NotifiableProperty HasJamProperty { get; }
        public bool HasJam => Model.HasJam;

        public JamSaveViewModel SaveSystem { get; }

        // -------
        // Loading
        // -------

        public ICommand OpenJamDirectoryCommand { get; }
        private async void OpenJamDirectory()
        {
            var directoryPath = FileQuery.OpenDirectory().GetPath();
            if (directoryPath == null)
                return;

            await LoadJamDirectory(directoryPath.Value);
        }

        public async Task LoadJamDirectory(FilePath directoryPath)
        {
            await Model.LoadDirectory(directoryPath);
            UpdateJamViewModel();
        }

        private void UpdateJamViewModel()
        {
            Jam = new JamEditableViewModel(Model.Jam!);
            SaveSystem.TrackViewModel(Jam);
            RaisePropertyChanged(nameof(Jam), nameof(HasJam));
        }

        // -------
        // Closing
        // -------

        public ICommand CloseJamCommand { get; }
        private void CloseJam()
        {
            if (!SaveSystem.TrySaveOnClose())
                return;

            Model.CloseJam();
            Jam = null;
            SaveSystem.DropViewModel();

            RaisePropertyChanged(nameof(Jam), nameof(HasJam));
        }

        // -------------
        // Compatibility
        // -------------

        public ICommand ExportCompatibilityCommand { get; }
        private void ExportCompatibility()
            => Model.ExportCompatibilityData();
    }
}
