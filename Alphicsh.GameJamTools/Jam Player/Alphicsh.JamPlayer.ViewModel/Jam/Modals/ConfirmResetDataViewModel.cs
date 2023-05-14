using System.Windows;
using System.Windows.Input;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.Modals;

namespace Alphicsh.JamPlayer.ViewModel.Jam.Modals
{
    public class ConfirmResetDataViewModel : ModalViewModel
    {
        public ConfirmResetDataViewModel()
            : base("Reset user data")
        {
            ResetUserDataCommand = SimpleCommand.From(ResetUserData);
        }

        public static void ShowModal()
        {
            var viewModel = new ConfirmResetDataViewModel();
            viewModel.ShowOwnModal();
        }

        public ICommand ResetUserDataCommand { get; }
        private void ResetUserData()
        {
            JamPlayerViewModel.Current.ResetUserData();
            Window.Close();
        }
    }
}
