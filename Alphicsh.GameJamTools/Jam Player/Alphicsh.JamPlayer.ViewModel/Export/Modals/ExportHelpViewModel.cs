using Alphicsh.JamTools.Common.Mvvm.Modals;

namespace Alphicsh.JamPlayer.ViewModel.Export.Modals
{
    public class ExportHelpViewModel : ModalViewModel
    {
        public ExportHelpViewModel()
            : base("Export options reference")
        {
        }

        public static void ShowModal()
        {
            var viewModel = new ExportHelpViewModel();
            viewModel.ShowOwnModal();
        }
    }
}
