using System.Windows.Input;
using Alphicsh.JamPackager.Model;
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
        }

        public NotifiableProperty HasJamProperty { get; }
        public bool HasJam => Model.HasJam;

        // -------
        // Loading
        // -------

        public ICommand OpenJamDirectoryCommand { get; }
        private void OpenJamDirectory()
        {
            var directoryPath = FileQuery.OpenDirectory().GetPath();
            if (directoryPath == null)
                return;

            LoadJamDirectory(directoryPath.Value);
        }

        public void LoadJamDirectory(FilePath directoryPath)
        {
            Model.LoadDirectory(directoryPath);
            UpdateJamViewModel();
        }

        private void UpdateJamViewModel()
        {
            RaisePropertyChanged(nameof(HasJam));
        }
    }
}
