using Alphicsh.EntryPackager.Controls.Preview.Modals;
using Alphicsh.JamPlayer.Controls.Export.Modals;
using Alphicsh.JamPlayer.Controls.Jam.Modals;
using Alphicsh.JamPlayer.Controls.Ranking.Modals;
using Alphicsh.JamPlayer.ViewModel.Export.Modals;
using Alphicsh.JamPlayer.ViewModel.Jam.Files.Modals;
using Alphicsh.JamPlayer.ViewModel.Jam.Modals;
using Alphicsh.JamPlayer.ViewModel.Ranking.Modals;
using Alphicsh.JamTools.Common.Mvvm.Modals;

namespace Alphicsh.JamPlayer.Controls
{
    public static class ModalsRegistration
    {
        public static void Register()
        {
            ModalWindowMapping.Add<ExportHelpViewModel, ExportHelpModal>();
            ModalWindowMapping.Add<ConfirmResetDataViewModel, ConfirmResetDataModal>();

            ModalWindowMapping.Add<SearchEntryViewModel, SearchEntryModal>();
            ModalWindowMapping.Add<PlaySelectionViewModel, PlaySelectionModal>();
        }
    }
}
