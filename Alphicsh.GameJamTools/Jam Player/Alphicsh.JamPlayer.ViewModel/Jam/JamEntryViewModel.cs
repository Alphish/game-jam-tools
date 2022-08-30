using System.Windows.Media;

using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

using Alphicsh.JamPlayer.Model.Jam;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.IO.Execution;
using System.Windows.Input;

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

            ThumbnailPathProperty = ImageSourceProperty.CreateReadonly(this, nameof(Thumbnail), vm => vm.Model.ThumbnailPath);
            ThumbnailSmallPathProperty = ImageSourceProperty.CreateReadonly(this, nameof(ThumbnailSmall), vm => vm.Model.ThumbnailSmallPath);

            Launcher = new ProcessLauncher();
            LaunchGameCommand = SimpleCommand.From(LaunchGame);
            OpenReadmeCommand = ReadmePath != null && !model.IsReadmePlease ? SimpleCommand.From(OpenReadme) : null;
            OpenReadmePleaseCommand = ReadmePath != null && model.IsReadmePlease ? SimpleCommand.From(OpenReadme) : null;
            OpenAfterwordCommand = AfterwordPath != null ? SimpleCommand.From(OpenAfterword) : null;
            OpenDirectoryCommand = SimpleCommand.From(OpenDirectory);
        }

        public string Title => Model.Title;
        public JamTeamViewModel Team { get; }

        // ------
        // Images
        // ------

        public ImageSourceProperty<JamEntryViewModel> ThumbnailPathProperty { get; }
        public ImageSource? Thumbnail => ThumbnailPathProperty.ImageSource;

        public ImageSourceProperty<JamEntryViewModel> ThumbnailSmallPathProperty { get; }
        public ImageSource? ThumbnailSmall => ThumbnailSmallPathProperty.ImageSource;

        // ---------
        // Execution
        // ---------

        private ProcessLauncher Launcher { get; }

        public FilePath? GamePath => Model.GamePath;
        public ICommand LaunchGameCommand { get; }
        private void LaunchGame()
        {
            if (GamePath == null)
                return;

            Launcher.OpenFile(GamePath.Value);
        }

        public FilePath? ReadmePath => Model.ReadmePath;
        public ICommand? OpenReadmeCommand { get; }
        public ICommand? OpenReadmePleaseCommand { get; }
        private void OpenReadme()
            => Launcher.OpenFile(ReadmePath!.Value);

        public FilePath? AfterwordPath => Model.AfterwordPath;
        public ICommand? OpenAfterwordCommand { get; }
        private void OpenAfterword()
            => Launcher.OpenFile(AfterwordPath!.Value);

        public FilePath DirectoryPath => Model.DirectoryPath;
        public ICommand OpenDirectoryCommand { get; }
        public void OpenDirectory()
        {
            Launcher.OpenDirectory(DirectoryPath);
        }
    }
}
