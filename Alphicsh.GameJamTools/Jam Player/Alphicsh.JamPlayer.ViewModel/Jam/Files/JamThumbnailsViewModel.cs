using System.Windows.Media;
using Alphicsh.JamPlayer.Model.Jam.Files;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPlayer.ViewModel.Jam.Files
{
    public class JamThumbnailsViewModel : WrapperViewModel<JamThumbnails>
    {
        public JamThumbnailsViewModel(JamThumbnails model) : base(model)
        {
            ThumbnailProperty = ImageSourceProperty.CreateReadonly(this, nameof(Thumbnail), vm => vm.Model.ThumbnailPath);
            ThumbnailSmallProperty = ImageSourceProperty.CreateReadonly(this, nameof(ThumbnailSmall), vm => vm.Model.ThumbnailSmallPath);
        }

        // ------
        // Images
        // ------

        public ImageSourceProperty<JamThumbnailsViewModel> ThumbnailProperty { get; }
        public ImageSource? Thumbnail => ThumbnailProperty.ImageSource ?? ThumbnailSmallProperty.ImageSource;

        public ImageSourceProperty<JamThumbnailsViewModel> ThumbnailSmallProperty { get; }
        public ImageSource? ThumbnailSmall => ThumbnailSmallProperty.ImageSource ?? ThumbnailProperty.ImageSource;
    }
}
