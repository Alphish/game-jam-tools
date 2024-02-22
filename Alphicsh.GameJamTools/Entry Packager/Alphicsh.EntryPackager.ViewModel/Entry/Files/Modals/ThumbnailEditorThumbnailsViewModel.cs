using System.IO;
using System;
using System.Windows.Media.Imaging;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using System.ComponentModel;
using System.Windows.Media;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Files.Modals
{
    public class ThumbnailEditorThumbnailsViewModel : BaseViewModel
    {
        public ThumbnailEditorThumbnailsViewModel(JamThumbnailsEditableViewModel thumbnails, ThumbnailEditorViewModel editor) 
        {
            Thumbnails = thumbnails;
            Thumbnails.PropertyChanged += OnThumbnailsPropertyChanged;

            Editor = editor;
            Editor.Crop.PropertyChanged += OnAreaPropertyChanged;

            SaveMainCommand = ConditionalCommand.From(CanSave, SaveMain);
            DeleteMainCommand = ConditionalCommand.From(CanDeleteMain, DeleteMain);
            SaveSmallCommand = ConditionalCommand.From(CanSave, SaveSmall);
            DeleteSmallCommand = ConditionalCommand.From(CanDeleteSmall, DeleteSmall);
        }

        private JamThumbnailsEditableViewModel Thumbnails { get; }
        public ImageSource? Main => Thumbnails.Thumbnail;
        public ImageSource? Small => Thumbnails.ThumbnailSmall;
        private void OnThumbnailsPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Thumbnails.Thumbnail))
                RaisePropertyChanged(nameof(Main));

            if (e.PropertyName == nameof(Thumbnails.ThumbnailSmall))
                RaisePropertyChanged(nameof(Small));
        }

        private ThumbnailEditorViewModel Editor { get; }
        private BitmapSource? CropPreview => Editor.Crop.CropPreview;
        private bool CanSave() => CropPreview != null;
        private void OnAreaPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Editor.Crop.CropPreview))
            {
                SaveMainCommand.RaiseCanExecuteChanged();
                SaveSmallCommand.RaiseCanExecuteChanged();
            }
        }

        // --------------
        // Main thumbnail
        // --------------

        public IConditionalCommand SaveMainCommand { get; }

        private void SaveMain()
        {
            if (CropPreview == null)
                return;

            var defaultLocation = Thumbnails.GetFullLocation("thumbnail.png")!.Value;
            var savePath = Thumbnails.ThumbnailFullLocation ?? defaultLocation;
            var isSaved = TrySave(CropPreview, savePath);
            if (!isSaved)
                return;

            if (Thumbnails.ThumbnailLocation == null)
                Thumbnails.ThumbnailLocation = savePath.GetLastSegmentName();

            Thumbnails.ThumbnailProperty.RaisePropertyChanged();
            Thumbnails.ThumbnailSmallProperty.RaisePropertyChanged();
            DeleteMainCommand.RaiseCanExecuteChanged();
        }

        public IConditionalCommand DeleteMainCommand { get; }

        private bool CanDeleteMain()
        {
            return Thumbnails.ThumbnailLocation != null;
        }

        private void DeleteMain()
        {
            if (Thumbnails.ThumbnailLocation == null)
                return;

            var thumbnailPath = Thumbnails.ThumbnailFullLocation!.Value;
            File.Delete(thumbnailPath.Value);
            Thumbnails.ThumbnailLocation = null;
            DeleteMainCommand.RaiseCanExecuteChanged();
        }

        // ---------------
        // Small thumbnail
        // ---------------

        public IConditionalCommand SaveSmallCommand { get; }

        private void SaveSmall()
        {
            if (CropPreview == null)
                return;

            var currentLocation = Thumbnails.ThumbnailSmallFullLocation != Thumbnails.ThumbnailFullLocation
                ? Thumbnails.ThumbnailSmallFullLocation
                : null;
            var defaultLocation = Thumbnails.GetFullLocation("thumbnail_small.png")!.Value;
            var savePath = currentLocation ?? defaultLocation;
            var isSaved = TrySave(CropPreview, savePath);
            if (!isSaved)
                return;

            if (currentLocation == null)
                Thumbnails.ThumbnailSmallLocation = savePath.GetLastSegmentName();

            Thumbnails.ThumbnailProperty.RaisePropertyChanged();
            Thumbnails.ThumbnailSmallProperty.RaisePropertyChanged();
            DeleteSmallCommand.RaiseCanExecuteChanged();
        }

        public IConditionalCommand DeleteSmallCommand { get; }

        private bool CanDeleteSmall()
        {
            return Thumbnails.ThumbnailSmallLocation != null;
        }

        private void DeleteSmall()
        {
            if (Thumbnails.ThumbnailSmallLocation == null)
                return;

            var thumbnailPath = Thumbnails.ThumbnailSmallFullLocation!.Value;
            File.Delete(thumbnailPath.Value);
            Thumbnails.ThumbnailSmallLocation = null;
            DeleteSmallCommand.RaiseCanExecuteChanged();
        }

        // ---------
        // Utilities
        // ---------

        internal static bool TrySave(BitmapSource source, FilePath path)
        {
            try
            {
                using var saveStream = new FileStream(path.Value, FileMode.Create);
                var encoder = new PngBitmapEncoder();
                var frame = BitmapFrame.Create(source);
                encoder.Frames.Add(frame);
                encoder.Save(saveStream);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
