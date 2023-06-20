using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPlayer.ViewModel.Jam.Files.Modals
{
    public class PlaySelectionItemViewModel : BaseViewModel
    {
        private PlaySelectionViewModel Selection { get; }
        public LaunchData LaunchData { get; }

        public PlaySelectionItemViewModel(PlaySelectionViewModel selection, LaunchData launchData)
        {
            Selection = selection;
            LaunchData = launchData;
            IsSelectedProperty = NotifiableProperty.Create(this, nameof(IsSelected));
        }

        public string Name => LaunchData.Name;
        public string? Description => LaunchData.Description;
        public bool HasDescription => LaunchData.Description != null;

        public NotifiableProperty IsSelectedProperty { get; }
        public bool IsSelected
        {
            get => Selection.SelectedItem == this;
            set
            {
                if (value != true)
                    return;

                Selection.SelectItem(this);
            }
        }
    }
}
