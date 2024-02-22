using System.Windows.Input;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.Modals;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Files.Modals
{
    public class ThumbnailEditorViewModel : ModalViewModel
    {
        public ThumbnailEditorViewModel(JamThumbnailsEditableViewModel thumbnails) : base("Make thumbnail")
        {
            Crop = new ThumbnailEditorCropViewModel();
            Source = new ThumbnailEditorSourceViewModel(thumbnails.Model.Files.DirectoryPath, Crop);
            Thumbnails = new ThumbnailEditorThumbnailsViewModel(thumbnails, this);

            IsHelpOpenedProperty = MutableProperty.Create(this, nameof(IsHelpOpened), false);
            OpenHelpCommand = SimpleCommand.From(() => IsHelpOpened = true);
            CloseHelpCommand = SimpleCommand.From(() => IsHelpOpened = false);

            Source.PasteCommand.Execute(null);
        }

        public static void ShowModal(JamThumbnailsEditableViewModel thumbnails)
        {
            var viewModel = new ThumbnailEditorViewModel(thumbnails);
            viewModel.ShowOwnModal();
        }

        public ThumbnailEditorCropViewModel Crop { get; }
        public ThumbnailEditorSourceViewModel Source { get; }
        public ThumbnailEditorThumbnailsViewModel Thumbnails { get; }

        public MutableProperty<bool> IsHelpOpenedProperty { get; }
        public bool IsHelpOpened { get => IsHelpOpenedProperty.Value; set => IsHelpOpenedProperty.Value = value; }
        public ICommand OpenHelpCommand { get; }
        public ICommand CloseHelpCommand { get; }
    }
}
