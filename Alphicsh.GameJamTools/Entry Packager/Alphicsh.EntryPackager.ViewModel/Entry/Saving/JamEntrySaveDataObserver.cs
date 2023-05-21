using Alphicsh.EntryPackager.ViewModel.Entry.Files;
using Alphicsh.JamTools.Common.Mvvm.Saving;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Saving
{
    internal class JamEntrySaveDataObserver : SaveDataObserver<JamEntryEditableViewModel>
    {
        public override void ObserveViewModel(JamEntryEditableViewModel viewModel)
        {
            ObserveProperty(viewModel.TitleProperty);
            ObserveTeam(viewModel.Team);
            ObserveFiles(viewModel.Files);
        }

        // ----
        // Team
        // ----

        private void ObserveTeam(JamTeamEditableViewModel team)
        {
            ObserveProperty(team.NameProperty);
            ObserveCollection(team.Authors, ObserveAuthor);
        }

        private void ObserveAuthor(JamAuthorEditableViewModel author)
        {
            ObserveProperty(author.NameProperty);
            ObserveProperty(author.CommunityIdProperty);
            ObserveProperty(author.RoleProperty);
        }

        // -----
        // Files
        // -----

        private void ObserveFiles(JamFilesEditableViewModel files)
        {
            ObserveCollection(files.Launchers, ObserveLauncher);
            ObserveReadme(files.Readme);
            ObserveAfterword(files.Afterword);
            ObserveThumbnails(files.Thumbnails);
        }

        private void ObserveLauncher(JamLauncherEditableViewModel launcher)
        {
            ObserveProperty(launcher.NameProperty);
            ObserveProperty(launcher.DescriptionProperty);
            ObserveProperty(launcher.TypeProperty);
            ObserveProperty(launcher.LocationProperty);
        }

        private void ObserveReadme(JamReadmeEditableViewModel readme)
        {
            ObserveProperty(readme.LocationProperty);
            ObserveProperty(readme.IsRequiredProperty);
        }

        private void ObserveAfterword(JamAfterwordEditableViewModel afterword)
        {
            ObserveProperty(afterword.LocationProperty);
        }

        private void ObserveThumbnails(JamThumbnailsEditableViewModel thumbnails)
        {
            ObserveProperty(thumbnails.ThumbnailLocationProperty);
            ObserveProperty(thumbnails.ThumbnailSmallLocationProperty);
        }
    }
}
