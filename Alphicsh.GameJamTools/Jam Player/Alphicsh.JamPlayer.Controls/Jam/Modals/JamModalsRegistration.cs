using Alphicsh.JamPlayer.ViewModel.Jam.Modals;
using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.JamPlayer.Controls.Jam.Modals
{
    public static class JamModalsRegistration
    {
        public static void Register()
        {
            JamModals.ConfirmResetDataCommand = SimpleCommand.From(ConfirmResetDataModal.ShowModal);
        }
    }
}
