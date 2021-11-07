using System.Windows.Media;

using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

using Alphicsh.JamPlayer.Model.Jam;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.IO.Execution;

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
            LaunchGameCommand = new SimpleCommand(LaunchGame);
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
        public SimpleCommand LaunchGameCommand { get; }
        private void LaunchGame()
        {
            if (GamePath == null)
                return;

            Launcher.OpenFile(GamePath.Value);
        }
    }
}
