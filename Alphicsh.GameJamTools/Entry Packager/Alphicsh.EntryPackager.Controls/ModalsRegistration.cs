using Alphicsh.EntryPackager.Controls.BasicInfo;
using Alphicsh.EntryPackager.Controls.Files.Modals;
using Alphicsh.EntryPackager.Controls.Preview.Modals;
using Alphicsh.EntryPackager.ViewModel.Entry;
using Alphicsh.EntryPackager.ViewModel.Entry.Files.Modals;
using Alphicsh.EntryPackager.ViewModel.Entry.Preview.Modals;
using Alphicsh.JamTools.Common.Controls.Modals;
using Alphicsh.JamTools.Common.Mvvm.Modals;

namespace Alphicsh.EntryPackager.Controls
{
    public static class ModalsRegistration
    {
        public static void Register()
        {
            ModalWindowMapping.Add<SimpleMessageViewModel, SimpleMessageModal>();
            ModalWindowMapping.Add<SaveOnCloseViewModel, SaveOnCloseModal>();

            ModalWindowMapping.Add<PlaySelectionViewModel, PlaySelectionModal>();

            ModalWindowMapping.Add<AttributionGuideViewModel, AttributionGuideModal>();

            ModalWindowMapping.Add<ThumbnailEditorViewModel, ThumbnailEditorModal>();
        }
    }
}
