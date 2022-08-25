using System.Windows;
using Alphicsh.JamPlayer.ViewModel.Export.Modals;

namespace Alphicsh.JamPlayer.Controls.Export.Modals
{
    /// <summary>
    /// Interaction logic for ExportHelpModal.xaml
    /// </summary>
    public partial class ExportHelpModal : Window
    {
        public ExportHelpModal()
        {
            InitializeComponent();
        }

        public static void ShowHelp()
        {
            var modal = new ExportHelpModal();
            var viewModel = new ExportHelpViewModel(modal);
            Modal.Show(modal, viewModel);
        }
    }
}
