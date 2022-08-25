using Alphicsh.JamPlayer.ViewModel.Export.Modals;
using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.JamPlayer.Controls.Export.Modals
{
    public static class ExportModalsRegistration
    {
        public static void Register()
        {
            ExportModals.ShowHelpCommand = SimpleCommand.From(ExportHelpModal.ShowModal);
        }
    }
}
