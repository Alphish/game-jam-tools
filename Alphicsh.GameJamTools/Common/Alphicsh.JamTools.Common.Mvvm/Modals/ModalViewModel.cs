using System.Windows;
using System.Windows.Input;
using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.JamTools.Common.Mvvm.Modals
{
    public abstract class ModalViewModel : BaseViewModel
    {
        public string Caption { get; }

        public Window Window { get; private set; } = default!;
        public ICommand CloseWindowCommand { get; private set; } = default!;

        protected ModalViewModel(string caption)
        {
            Caption = caption;
        }

        protected bool ShowOwnModal()
        {
            Window = ModalWindowMapping.CreateWindowFor(this.GetType());
            Window.DataContext = this;
            CloseWindowCommand = SimpleCommand.From(Window.Close);

            AppViewModel.Current.HasOverlay = true;
            Window.Owner = AppWindow.Current;
            Window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            var result = Window.ShowDialog();

            AppViewModel.Current.HasOverlay = false;

            return result ?? false;
        }
    }
}
