using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using Alphicsh.EntryPackager.Model.Entry.Files;
using Alphicsh.EntryPackager.ViewModel.Entry.Files.Modals;
using Alphicsh.JamTools.Common.Controls.Files;
using Alphicsh.JamTools.Common.IO;
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
                .DependingOn(ThumbnailLocationProperty, ThumbnailSmallLocationProperty);

            ThumbnailProperty = ImageSourceProperty
                .CreateReadonly(this, nameof(Thumbnail), vm => vm.Model.ThumbnailFullLocation ?? vm.Model.ThumbnailSmallFullLocation)
                .DependingOn(ThumbnailLocationProperty, ThumbnailSmallLocationProperty);
            ThumbnailSmallProperty = ImageSourceProperty
                .CreateReadonly(this, nameof(ThumbnailSmall), vm => vm.Model.ThumbnailSmallFullLocation ?? vm.Model.ThumbnailFullLocation)
                .DependingOn(ThumbnailLocationProperty, ThumbnailSmallLocationProperty);

            MakeThumbnailCommand = SimpleCommand.From(MakeThumbnail);
            SearchThumbnailCommand = SimpleCommand.From(SearchThumbnail);
            SearchThumbnailSmallCommand = SimpleCommand.From(SearchThumbnailSmall);
        }

        // --------
        // Location
        // --------

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

        internal FilePath? ThumbnailFullLocation => Model.ThumbnailFullLocation;
        internal FilePath? ThumbnailSmallFullLocation => Model.ThumbnailSmallFullLocation;
        internal FilePath? GetFullLocation(string? location) => Model.GetFullLocation(location);

        // ------
        // Images
        // ------

        public ImageSourceProperty<JamThumbnailsEditableViewModel> ThumbnailProperty { get; }
        public ImageSource? Thumbnail => ThumbnailProperty.ImageSource;

        public ImageSourceProperty<JamThumbnailsEditableViewModel> ThumbnailSmallProperty { get; }
        public ImageSource? ThumbnailSmall => ThumbnailSmallProperty.ImageSource;

        // ------
        // Making
        // ------

        public ICommand MakeThumbnailCommand { get; }
        private void MakeThumbnail() => ThumbnailEditorViewModel.ShowModal(this);

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
