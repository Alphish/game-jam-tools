using System.Windows.Media;
using System.Windows.Media.Imaging;

using Alphicsh.JamTools.Common.Mvvm;

using Alphicsh.JamPlayer.Model.Jam;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

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
        }

        public string Title => Model.Title;
        public JamTeamViewModel Team { get; }

        public ImageSourceProperty<JamEntryViewModel> ThumbnailPathProperty { get; }
        public ImageSource? Thumbnail => ThumbnailPathProperty.ImageSource;

        public ImageSourceProperty<JamEntryViewModel> ThumbnailSmallPathProperty { get; }
        public ImageSource? ThumbnailSmall => ThumbnailSmallPathProperty.ImageSource;
    }
}
