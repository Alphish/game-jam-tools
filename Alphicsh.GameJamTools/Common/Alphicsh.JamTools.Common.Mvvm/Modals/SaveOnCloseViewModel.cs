using System.Windows.Input;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.Saving;

namespace Alphicsh.JamTools.Common.Mvvm.Modals
{
    public class SaveOnCloseViewModel : ModalViewModel
    {
        private ISaveViewModel SaveViewModel { get; }

        public SaveOnCloseViewModel(ISaveViewModel saveViewModel) : base("Unsaved changes")
        {
            SaveViewModel = saveViewModel;

            SaveAndCloseCommand = SimpleCommand.From(SaveAndClose);
            OnlyCloseCommand = SimpleCommand.From(OnlyClose);
            CancelCommand = SimpleCommand.From(Cancel);
        }

        public static bool IsPromptShown { get; private set; }

        public static bool ShowModal(ISaveViewModel saveViewModel)
        {
            IsPromptShown = true;
            var viewModel = new SaveOnCloseViewModel(saveViewModel);
            IsPromptShown = false;
            return viewModel.ShowOwnModal();
        }

        public ICommand SaveAndCloseCommand { get; }
        private void SaveAndClose()
        {
            SaveViewModel.Save();
            Window.DialogResult = true;
            Window.Close();
        }

        public ICommand OnlyCloseCommand { get; }
        private void OnlyClose()
        {
            Window.DialogResult = true;
            Window.Close();
        }

        public ICommand CancelCommand { get; }
        private void Cancel()
        {
            Window.Close();
        }
    }
}
