using System;
using System.Windows.Input;
using System.Windows.Media;
using Alphicsh.JamPlayer.Model.Jam;
using Alphicsh.JamPlayer.ViewModel.Jam.Files;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.JamPlayer.ViewModel.Jam
{
    public sealed class JamEntryViewModel : WrapperViewModel<JamEntry>
    {
        public static CollectionViewModelStub<JamEntry, JamEntryViewModel> CollectionStub { get; }
            = CollectionViewModelStub.Create((JamEntry model) => new JamEntryViewModel(model));

        private static ProcessLauncher ProcessLauncher { get; } = new ProcessLauncher();

        public JamEntryViewModel(JamEntry model)
            : base(model)
        {
            Team = new JamTeamViewModel(model.Team);
            Files = new JamFilesViewModel(model.Files);

            OpenPageCommand = SimpleCommand.From(OpenPage);
        }

        public string Title => Model.Title;
        public JamTeamViewModel Team { get; }
        public string AttributionString => Model.Attribution.AttributionString;
        public bool AttributionSpecified => JamPlayerViewModel.Current.Jam.AttributionSpecified;
        public JamFilesViewModel Files { get; }

        public string? PageUrl => Model.PageUrl;
        public bool HasPageUrl => !string.IsNullOrWhiteSpace(PageUrl) && Uri.TryCreate(PageUrl, UriKind.Absolute, out var pageUri);
        public ICommand OpenPageCommand { get; }
        private void OpenPage()
        {
            if (string.IsNullOrWhiteSpace(PageUrl) || !Uri.TryCreate(PageUrl, UriKind.Absolute, out var pageUri))
                return;

            ProcessLauncher.OpenWebsite(pageUri);
        }

        // ------
        // Images
        // ------

        public ImageSource? Thumbnail => Files.Thumbnail;
        public ImageSource? ThumbnailSmall => Files.ThumbnailSmall;

        // ---------
        // Execution
        // ---------

        public string PlayDescription => Files.PlayDescription;
        public ICommand LaunchGameCommand => Files.PlayCommand;

        public bool HasRequiredReadme => Files.HasRequiredReadme;
        public bool HasRegularReadme => Files.HasRegularReadme;
        public ICommand? OpenReadmeCommand => Files.OpenReadmeCommand;

        public bool HasAfterword => Files.HasAfterword;
        public ICommand? OpenAfterwordCommand => Files.OpenAfterwordCommand;

        public ICommand OpenDirectoryCommand => Files.OpenDirectoryCommand;
    }
}
