using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Alphicsh.EntryPackager.ViewModel.Entry.Files;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.Modals;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Preview.Modals
{
    public class PlaySelectionViewModel : ModalViewModel
    {
        public PlaySelectionViewModel(JamLaunchersCollectionViewModel launchers) : base("Select version")
        {
            Items = launchers.Select(launcher => new PlaySelectionItemViewModel(this, launcher)).ToList();
            PlayCommand = SimpleCommand.From(Play);

            SelectedItem = Items.First();
            foreach (var item in Items)
            {
                item.IsSelectedProperty.RaisePropertyChanged();
            }
        }

        public static void ShowModal(JamLaunchersCollectionViewModel launchers)
        {
            var viewModel = new PlaySelectionViewModel(launchers);
            viewModel.ShowOwnModal();
        }

        public IReadOnlyCollection<PlaySelectionItemViewModel> Items { get; }
        public PlaySelectionItemViewModel SelectedItem { get; set; }
        public void SelectItem(PlaySelectionItemViewModel selectedItem)
        {
            SelectedItem = selectedItem;
            foreach (var item in Items)
            {
                item.IsSelectedProperty.RaisePropertyChanged();
            }
        }

        public ICommand PlayCommand { get; }
        private void Play()
        {
            SelectedItem.Launcher.LaunchCommand.Execute(null);
            Window.Close();
        }
    }
}
