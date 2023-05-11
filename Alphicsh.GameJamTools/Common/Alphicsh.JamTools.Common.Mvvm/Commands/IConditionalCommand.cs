using System.Windows.Input;

namespace Alphicsh.JamTools.Common.Mvvm.Commands
{
    public interface IConditionalCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
