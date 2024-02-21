using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Alphicsh.EntryPackager.Model.Entry.Files;
using Alphicsh.EntryPackager.ViewModel.Entry.Preview.Modals;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Files
{
    public class JamLaunchersCollectionViewModel : CollectionViewModel<JamLauncherEditable, JamLauncherEditableViewModel>
    {
        public JamLaunchersCollectionViewModel(IList<JamLauncherEditable> modelEntries)
            : base(modelEntries, JamLauncherEditableViewModel.CollectionStub, isImmutable: false)
        {
            HasLaunchersProperty = NotifiableProperty.Create(this, nameof(HasLaunchers));
            PlayDescriptionProperty = NotifiableProperty.Create(this, nameof(PlayDescription));
            PlayCommand = SimpleCommand.From(Play);
        }

        // ----------------
        // Change detection
        // ----------------

        private ICollection<JamLauncherEditableViewModel> TrackedViewModels { get; } = new List<JamLauncherEditableViewModel>();

        protected override void ApplyChanges()
        {
            foreach (var viewModel in TrackedViewModels)
            {
                viewModel.PropertyChanged -= OnLauncherPropertyChange;
            }
            TrackedViewModels.Clear();

            foreach (var viewModel in ViewModels)
            {
                viewModel.PropertyChanged += OnLauncherPropertyChange;
                TrackedViewModels.Add(viewModel);
            }

            HasLaunchersProperty?.RaisePropertyChanged();
            PlayDescriptionProperty?.RaisePropertyChanged();
        }

        private void OnLauncherPropertyChange(object? sender, EventArgs e)
        {
            PlayDescriptionProperty?.RaisePropertyChanged();
        }

        // ----------
        // Properties
        // ----------

        public NotifiableProperty HasLaunchersProperty { get; }
        public bool HasLaunchers => Models.Any();

        public NotifiableProperty PlayDescriptionProperty { get; }
        public string PlayDescription
        {
            get
            {
                if (!Models.Any())
                    return "No launcher";
                else if (Models.Count > 1)
                    return "Play...";
                else if (Models.First().Type == LaunchType.GxGamesLink)
                    return "Play (GX)";
                else if (Models.First().Type == LaunchType.WebLink)
                    return "Play (Web)";
                else
                    return "Play";
            }
        }

        public ICommand PlayCommand { get; }
        private void Play()
        {
            if (ViewModels.Count == 0)
                return;
            else if (ViewModels.Count == 1)
                ViewModels.First().LaunchCommand.Execute(null);
            else
                PlaySelectionViewModel.ShowModal(this);
        }
    }
}
