using Alphicsh.JamPlayer.Controls.Export.Modals;
using Alphicsh.JamPlayer.Controls.Jam.Modals;

namespace Alphicsh.JamPlayer.Controls
{
    public static class ModalsRegistration
    {
        public static void Register()
        {
            ExportModalsRegistration.Register();
            JamModalsRegistration.Register();
        }
    }
}
