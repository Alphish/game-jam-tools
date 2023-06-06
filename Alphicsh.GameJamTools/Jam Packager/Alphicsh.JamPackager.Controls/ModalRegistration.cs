using Alphicsh.EntryPackager.Controls.Preview.Modals;
using Alphicsh.EntryPackager.ViewModel.Entry.Files.Modals;
using Alphicsh.JamTools.Common.Controls.Modals;
using Alphicsh.JamTools.Common.Mvvm.Modals;

namespace Alphicsh.JamPackager.Controls
{
    public static class ModalsRegistration
    {
        public static void Register()
        {
            ModalWindowMapping.Add<SimpleMessageViewModel, SimpleMessageModal>();
            ModalWindowMapping.Add<SaveOnCloseViewModel, SaveOnCloseModal>();

            ModalWindowMapping.Add<PlaySelectionViewModel, PlaySelectionModal>();
        }
    }
}
