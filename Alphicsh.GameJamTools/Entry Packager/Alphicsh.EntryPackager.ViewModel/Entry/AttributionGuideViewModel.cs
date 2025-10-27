using Alphicsh.JamTools.Common.Mvvm.Modals;

namespace Alphicsh.EntryPackager.ViewModel.Entry
{
    public class AttributionGuideViewModel : ModalViewModel
    {
        public AttributionGuideViewModel()
            : base("Attribution guide")
        {
        }

        public static void ShowModal()
        {
            var viewModel = new AttributionGuideViewModel();
            viewModel.ShowOwnModal();
        }
    }
}
