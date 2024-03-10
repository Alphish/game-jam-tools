using Alphicsh.EntryPackager.ViewModel.Entry.Files;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Preview.Modals
{
    public class PlaySelectionItemViewModel : BaseViewModel
    {
        private PlaySelectionViewModel Selection { get; }
        public JamLauncherEditableViewModel Launcher { get; }

        public PlaySelectionItemViewModel(PlaySelectionViewModel selection, JamLauncherEditableViewModel launcher)
        {
            Selection = selection;
            Launcher = launcher;
            IsSelectedProperty = NotifiableProperty.Create(this, nameof(IsSelected));
        }

        public string Name => Launcher.Name;
        public string? Description => Launcher.Description;
        public bool HasDescription => Launcher.Description != null;

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
