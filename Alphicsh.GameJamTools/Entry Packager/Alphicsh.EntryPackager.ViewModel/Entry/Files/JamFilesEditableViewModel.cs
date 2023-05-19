using System.Collections.Generic;
using System.Windows.Input;
using Alphicsh.EntryPackager.Model.Entry.Files;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Files
{
    public class JamFilesEditableViewModel : WrapperViewModel<JamFilesEditable>
    {
        public JamFilesEditableViewModel(JamFilesEditable model) : base(model)
        {
            Launchers = CollectionViewModel.CreateMutable(model.Launchers, JamLauncherEditableViewModel.CollectionStub);
            Readme = new JamReadmeEditableViewModel(model.Readme);
            Afterword = new JamAfterwordEditableViewModel(model.Afterword);
            Thumbnails = new JamThumbnailsEditableViewModel(model.Thumbnails);

            AddLauncherCommand = SimpleCommand.From(AddLauncher);
            RemoveLauncherCommand = SimpleCommand.WithParameter<JamLauncherEditableViewModel>(RemoveLauncher);

            ClearInvalidCommand = SimpleCommand.From(ClearInvalid);
            RediscoverFilesCommand = SimpleCommand.From(RediscoverFiles);
        }

        public CollectionViewModel<JamLauncherEditable, JamLauncherEditableViewModel> Launchers { get; }
        public JamReadmeEditableViewModel Readme { get; }
        public JamAfterwordEditableViewModel Afterword { get; }
        public JamThumbnailsEditableViewModel Thumbnails { get; }

        // ---------
        // Launchers
        // ---------

        public ICommand AddLauncherCommand { get; }
        private void AddLauncher()
        {
            var launcherModel = new JamLauncherEditable(Model);
            var launcherVm = new JamLauncherEditableViewModel(launcherModel);
            Launchers.Add(launcherVm);
            Launchers.CompleteChanges();
        }

        public ICommand RemoveLauncherCommand { get; }
        private void RemoveLauncher(JamLauncherEditableViewModel launcherVm)
        {
            Launchers.Remove(launcherVm);
            Launchers.CompleteChanges();
        }

        public IReadOnlyCollection<LaunchTypeEntry> AvailableLaunchTypes { get; } = new List<LaunchTypeEntry>()
        {
            new LaunchTypeEntry("Executable file", LaunchType.WindowsExe),
            new LaunchTypeEntry("Web page", LaunchType.WebLink),
            new LaunchTypeEntry("GX.games page", LaunchType.GxGamesLink),
        };

        // -------
        // Actions
        // -------

        public ICommand ClearInvalidCommand { get; }
        private void ClearInvalid()
        {
            Model.ClearInvalid();
            NotifyCoreProperties();
        }

        public ICommand RediscoverFilesCommand { get; }
        private void RediscoverFiles()
        {
            Model.Rediscover();
            NotifyCoreProperties();
        }

        private void NotifyCoreProperties()
        {
            Launchers.SynchronizeWithModels();
            Readme.LocationProperty.RaisePropertyChanged();
            Readme.IsRequiredProperty.RaisePropertyChanged();
            Afterword.LocationProperty.RaisePropertyChanged();
            Thumbnails.ThumbnailLocationProperty.RaisePropertyChanged();
            Thumbnails.ThumbnailSmallLocationProperty.RaisePropertyChanged();
        }
    }
}
