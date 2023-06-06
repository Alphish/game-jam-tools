using Alphicsh.JamPlayer.Model.Jam.Files;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using System.Windows.Input;
using Alphicsh.JamTools.Common.Mvvm;

namespace Alphicsh.JamPlayer.ViewModel.Jam.Files
{
    public class JamAfterwordViewModel : WrapperViewModel<JamAfterword>
    {
        private static ProcessLauncher ProcessLauncher { get; } = new ProcessLauncher();

        public JamAfterwordViewModel(JamAfterword model) : base(model)
        {
            OpenAfterwordCommand = SimpleCommand.From(OpenAfterword);
        }

        public ICommand OpenAfterwordCommand { get; }
        private void OpenAfterword()
        {
            ProcessLauncher.OpenFile(Model.Path);
        }
    }
}
