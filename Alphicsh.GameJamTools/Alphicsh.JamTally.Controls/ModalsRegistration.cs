using Alphicsh.JamTally.Controls.Vote.Modals;
using Alphicsh.JamTally.ViewModel.Vote.Modals;
using Alphicsh.JamTools.Common.Controls.Modals;
using Alphicsh.JamTools.Common.Mvvm.Modals;

namespace Alphicsh.JamTally.Controls
{
    public class ModalsRegistration
    {
        public static void Register()
        {
            ModalWindowMapping.Add<SimpleMessageViewModel, SimpleMessageModal>();
            ModalWindowMapping.Add<SaveOnCloseViewModel, SaveOnCloseModal>();

            ModalWindowMapping.Add<VoteEntriesEditorViewModel, VoteEntriesEditorModal>();
        }
    }
}
