using System.Windows;
using System.Windows.Input;
using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.JamTools.Common.Mvvm.Modals
{
    public abstract class ModalViewModel : BaseViewModel
    {
        public string Caption { get; }

        public Window Window { get; }
        public ICommand CloseWindowCommand { get; }

        protected ModalViewModel(Window window, string caption)
        {
            Caption = caption;

            Window = window;
            CloseWindowCommand = SimpleCommand.From(Window.Close);
        }
    }
}
