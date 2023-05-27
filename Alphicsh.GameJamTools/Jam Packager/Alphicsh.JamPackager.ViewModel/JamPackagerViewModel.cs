using System.Windows.Input;
using Alphicsh.JamPackager.Model;
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
            DirectoryPathProperty = new MutableProperty<string>(this, nameof(DirectoryPath), string.Empty);

            GenerateJamFilesCommand = SimpleCommand.From(GenerateJamFiles);
        }

        public MutableProperty<string> DirectoryPathProperty { get; }
        public string DirectoryPath { get => DirectoryPathProperty.Value; set => DirectoryPathProperty.Value = value; }

        public ICommand GenerateJamFilesCommand { get; }
        private void GenerateJamFiles()
        {
            Model.LoadDirectory(FilePath.From(DirectoryPath));
            // will need to remove all this saving eventually
            /*
            var jamDirectoryPath = FilePath.From(DirectoryPath);

            var jamInfo = JamInfo.RediscoverFromDirectory(jamDirectoryPath);
            jamInfo.Save();
            */
        }
    }
}
