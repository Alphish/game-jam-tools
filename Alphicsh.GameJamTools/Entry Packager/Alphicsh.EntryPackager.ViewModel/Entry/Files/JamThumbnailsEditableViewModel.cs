using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using Alphicsh.EntryPackager.Model.Entry.Files;
using Alphicsh.JamTools.Common.Controls.Files;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Files
{
    public class JamThumbnailsEditableViewModel : WrapperViewModel<JamThumbnailsEditable>
    {
        public JamThumbnailsEditableViewModel(JamThumbnailsEditable model) : base(model)
        {
            ThumbnailLocationProperty = WrapperProperty.ForMember(this, vm => vm.Model.ThumbnailLocation);
            ThumbnailSmallLocationProperty = WrapperProperty.ForMember(this, vm => vm.Model.ThumbnailSmallLocation);

            ThumbnailLocationPlaceholderProperty = NotifiableProperty.Create(this, nameof(ThumbnailLocationPlaceholder))
                .DepeningOn(ThumbnailLocationProperty, ThumbnailSmallLocationProperty);

            ThumbnailProperty = ImageSourceProperty
                .CreateReadonly(this, nameof(Thumbnail), vm => vm.Model.ThumbnailFullLocation ?? vm.Model.ThumbnailSmallFullLocation)
                .DepeningOn(ThumbnailLocationProperty, ThumbnailSmallLocationProperty);
            ThumbnailSmallProperty = ImageSourceProperty
                .CreateReadonly(this, nameof(ThumbnailSmall), vm => vm.Model.ThumbnailSmallFullLocation ?? vm.Model.ThumbnailFullLocation)
                .DepeningOn(ThumbnailLocationProperty, ThumbnailSmallLocationProperty);

            SearchThumbnailCommand = SimpleCommand.From(SearchThumbnail);
            SearchThumbnailSmallCommand = SimpleCommand.From(SearchThumbnailSmall);
        }

        public WrapperProperty<JamThumbnailsEditableViewModel, string?> ThumbnailLocationProperty { get; }
        public string? ThumbnailLocation { get => ThumbnailLocationProperty.Value; set => ThumbnailLocationProperty.Value = value; }

        public WrapperProperty<JamThumbnailsEditableViewModel, string?> ThumbnailSmallLocationProperty { get; }
        public string? ThumbnailSmallLocation
        {
            get => ThumbnailSmallLocationProperty.Value;
            set => ThumbnailSmallLocationProperty.Value = value;
        }

        public NotifiableProperty ThumbnailLocationPlaceholderProperty { get; }
        public string ThumbnailLocationPlaceholder
            => Model.HasThumbnailLocation || Model.HasThumbnailSmallLocation ? "<same as other>" : "<no thumbnail>";

        // ------
        // Images
        // ------

        public ImageSourceProperty<JamThumbnailsEditableViewModel> ThumbnailProperty { get; }
        public ImageSource? Thumbnail => ThumbnailProperty.ImageSource;

        public ImageSourceProperty<JamThumbnailsEditableViewModel> ThumbnailSmallProperty { get; }
        public ImageSource? ThumbnailSmall => ThumbnailSmallProperty.ImageSource;

        // ---------
        // Searching
        // ---------

        public ICommand SearchThumbnailCommand { get; }
        private void SearchThumbnail() => SearchFor(ThumbnailLocationProperty, "thumbnail");

        public ICommand SearchThumbnailSmallCommand { get; }
        private void SearchThumbnailSmall() => SearchFor(ThumbnailSmallLocationProperty, "thumbnail_small");

        private void SearchFor(WrapperProperty<JamThumbnailsEditableViewModel, string?> property, string filename)
        {
            var entryDirectory = Model.Files.DirectoryPath;
            var path = FileQuery.OpenFile()
                .FromDirectory(entryDirectory)
                .WithFileType("*.png;*.jpg;*.jpeg;*.bmp", "Image files")
                .GetPath();

            if (path == null)
                return;

            var foundPath = path.Value;
            if (!foundPath.IsSubpathOf(entryDirectory))
            {
                var newPath = entryDirectory.Append(filename + foundPath.GetExtension());
                File.Copy(foundPath.Value, newPath.Value, overwrite: true);
                foundPath = newPath;
            }

            var subpath = foundPath.AsRelativeTo(entryDirectory);
            property.Value = subpath.Value;
        }
    }
}
