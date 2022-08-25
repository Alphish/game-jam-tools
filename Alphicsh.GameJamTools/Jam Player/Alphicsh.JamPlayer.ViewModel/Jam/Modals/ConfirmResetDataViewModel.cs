using System.Windows;
using System.Windows.Input;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.Modals;

namespace Alphicsh.JamPlayer.ViewModel.Jam.Modals
{
    public class ConfirmResetDataViewModel : ModalViewModel
    {
        public ConfirmResetDataViewModel(Window window)
            : base(window, "Reset user data")
        {
            ResetUserDataCommand = SimpleCommand.From(ResetUserData);
        }

        public ICommand ResetUserDataCommand { get; }
        private void ResetUserData()
        {
            AppViewModel.Current.ResetUserData();
            Window.Close();
        }
    }
}
