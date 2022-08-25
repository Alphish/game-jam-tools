using System.Windows;
using Alphicsh.JamPlayer.ViewModel;
using Alphicsh.JamTools.Common.Mvvm.Modals;

namespace Alphicsh.JamPlayer.Controls
{
    public static class Modal
    {
        public static void Show(Window modal, ModalViewModel viewModel)
        {
            AppViewModel.Current.HasOverlay = true;
            modal.Owner = MainWindow.Current;
            modal.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            modal.DataContext = viewModel;
            modal.ShowDialog();
            AppViewModel.Current.HasOverlay = false;
        }
    }
}
