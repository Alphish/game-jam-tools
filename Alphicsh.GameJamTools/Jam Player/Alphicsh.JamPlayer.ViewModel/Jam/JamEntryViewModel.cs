using System.Windows.Input;
using System.Windows.Media;
using Alphicsh.JamPlayer.Model.Jam;
using Alphicsh.JamPlayer.ViewModel.Jam.Files;
using Alphicsh.JamTools.Common.Mvvm;

namespace Alphicsh.JamPlayer.ViewModel.Jam
{
    public sealed class JamEntryViewModel : WrapperViewModel<JamEntry>
    {
        public static CollectionViewModelStub<JamEntry, JamEntryViewModel> CollectionStub { get; }
            = CollectionViewModelStub.Create((JamEntry model) => new JamEntryViewModel(model));

        public JamEntryViewModel(JamEntry model)
            : base(model)
        {
            Team = new JamTeamViewModel(model.Team);
            Files = new JamFilesViewModel(model.Files);
        }

        public string Title => Model.Title;
        public JamTeamViewModel Team { get; }
        public JamFilesViewModel Files { get; }

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
