using Alphicsh.EntryPackager.ViewModel.Entry.Files;
using Alphicsh.JamTools.Common.Mvvm.Observation;
using Alphicsh.JamTools.Common.Mvvm.Saving;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Saving
{
    public class JamEntrySaveDataObserver : SaveDataObserver<JamEntryEditableViewModel>
    {
        protected override IObserverNode CreateInnerObserver(JamEntryEditableViewModel viewModel)
        {
            return CreateViewModelObserver()
                .ObservingProperty(viewModel.TitleProperty)
                .ObservingProperty(viewModel.ShortTitleProperty)
                .ObservingViewModel(ObserveTeam(viewModel.Team))
                .ObservingViewModel(ObserveFiles(viewModel.Files));
        }

        // ----
        // Team
        // ----

        private IObserverNode ObserveTeam(JamTeamEditableViewModel team)
        {
            return CreateViewModelObserver()
                .ObservingProperty(team.NameProperty)
                .ObservingCollection(team.Authors, ObserveAuthor);
        }

        private IObserverNode ObserveAuthor(JamAuthorEditableViewModel author)
        {
            return CreateViewModelObserver()
                .ObservingProperty(author.NameProperty)
                .ObservingProperty(author.CommunityIdProperty)
                .ObservingProperty(author.RoleProperty);
        }

        // -----
        // Files
        // -----

        private IObserverNode ObserveFiles(JamFilesEditableViewModel files)
        {
            return CreateViewModelObserver()
                .ObservingCollection(files.Launchers, ObserveLauncher)
                .ObservingViewModel(ObserveReadme(files.Readme))
                .ObservingViewModel(ObserveAfterword(files.Afterword))
                .ObservingViewModel(ObserveThumbnails(files.Thumbnails));
        }

        private IObserverNode ObserveLauncher(JamLauncherEditableViewModel launcher)
        {
            return CreateViewModelObserver()
                .ObservingProperty(launcher.NameProperty)
                .ObservingProperty(launcher.DescriptionProperty)
                .ObservingProperty(launcher.TypeProperty)
                .ObservingProperty(launcher.LocationProperty);
        }

        private IObserverNode ObserveReadme(JamReadmeEditableViewModel readme)
        {
            return CreateViewModelObserver()
                .ObservingProperty(readme.LocationProperty)
                .ObservingProperty(readme.IsRequiredProperty);
        }

        private IObserverNode ObserveAfterword(JamAfterwordEditableViewModel afterword)
        {
            return CreateViewModelObserver()
                .ObservingProperty(afterword.LocationProperty);
        }

        private IObserverNode ObserveThumbnails(JamThumbnailsEditableViewModel thumbnails)
        {
            return CreateViewModelObserver()
                .ObservingProperty(thumbnails.ThumbnailLocationProperty)
                .ObservingProperty(thumbnails.ThumbnailSmallLocationProperty);
        }
    }
}
