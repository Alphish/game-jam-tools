using Alphicsh.JamTools.Common.Controls.Modals;
using Alphicsh.JamTools.Common.Mvvm.Modals;

namespace Alphicsh.EntryPackager.Controls
{
    public static class ModalsRegistration
    {
        public static void Register()
        {
            ModalWindowMapping.Add<SimpleMessageViewModel, SimpleMessageModal>();
        }
    }
}
