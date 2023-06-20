using System.Windows.Input;
using Alphicsh.JamPlayer.Model.Jam.Files;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.JamPlayer.ViewModel.Jam.Files
{
    public class JamReadmeViewModel : WrapperViewModel<JamReadme>
    {
        private static ProcessLauncher ProcessLauncher { get; } = new ProcessLauncher();

        public JamReadmeViewModel(JamReadme model) : base(model)
        {
            OpenReadmeCommand = SimpleCommand.From(OpenReadme);
        }

        public bool IsRequired => Model.IsRequired;

        public ICommand OpenReadmeCommand { get; }
        private void OpenReadme()
        {
            ProcessLauncher.OpenFile(Model.Path);
        }
    }
}
