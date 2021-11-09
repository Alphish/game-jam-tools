using System.Windows.Input;

using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Jam;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPackager.App
{
    // TODO: Move the whole view model to a separate project
    public class AppViewModel : BaseViewModel
    {
        public AppViewModel()
        {
            DirectoryPathProperty = new MutableProperty<string>(this, nameof(DirectoryPath), string.Empty);

            GenerateJamFilesCommand = new SimpleCommand(GenerateJamFiles);
        }

        public MutableProperty<string> DirectoryPathProperty { get; }
        public string DirectoryPath { get => DirectoryPathProperty.Value; set => DirectoryPathProperty.Value = value; }

        public ICommand GenerateJamFilesCommand { get; }
        private void GenerateJamFiles()
        {
            var jamDirectoryPath = FilePath.From(DirectoryPath);

            var jamInfo = JamInfo.RediscoverFromDirectory(jamDirectoryPath);
            jamInfo.Save();
        }
    }
}
