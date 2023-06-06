using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using System.Windows.Input;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.Mvvm.Modals;

namespace Alphicsh.JamPlayer.ViewModel.Jam.Files.Modals
{
    public class PlaySelectionViewModel : ModalViewModel
    {
        private static EntryLauncher EntryLauncher { get; } = new EntryLauncher();
        
        public PlaySelectionViewModel(IReadOnlyCollection<LaunchData> launchers) : base("Select version")
        {
            Items = launchers.Select(launcher => new PlaySelectionItemViewModel(this, launcher)).ToList();
            PlayCommand = SimpleCommand.From(Play);
            SelectItem(Items.First());
        }

        public static void ShowModal(IReadOnlyCollection<LaunchData> launchers)
        {
            var viewModel = new PlaySelectionViewModel(launchers);
            viewModel.ShowOwnModal();
        }

        public IReadOnlyCollection<PlaySelectionItemViewModel> Items { get; }
        public PlaySelectionItemViewModel SelectedItem { get; set; } = default!;
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
            EntryLauncher.Launch(SelectedItem.LaunchData);
            Window.Close();
        }
    }
}
