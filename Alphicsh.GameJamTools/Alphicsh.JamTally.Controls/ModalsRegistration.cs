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
        }
    }
}
