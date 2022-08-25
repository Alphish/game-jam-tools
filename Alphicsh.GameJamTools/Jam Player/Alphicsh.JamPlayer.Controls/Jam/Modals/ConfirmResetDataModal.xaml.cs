using System.Windows;
using Alphicsh.JamPlayer.ViewModel.Jam.Modals;

namespace Alphicsh.JamPlayer.Controls.Jam.Modals
{
    /// <summary>
    /// Interaction logic for ConfirmResetDataModal.xaml
    /// </summary>
    public partial class ConfirmResetDataModal : Window
    {
        public ConfirmResetDataModal()
        {
            InitializeComponent();
        }

        public static void ShowModal()
        {
            var modal = new ConfirmResetDataModal();
            var viewModel = new ConfirmResetDataViewModel(modal);
            Modal.Show(modal, viewModel);
        }
    }
}
