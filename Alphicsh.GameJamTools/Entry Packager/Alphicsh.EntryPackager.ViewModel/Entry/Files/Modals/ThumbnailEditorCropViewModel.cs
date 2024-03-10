using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Files.Modals
{
    public class ThumbnailEditorCropViewModel : BaseViewModel
    {
        public ThumbnailEditorCropViewModel()
        {
            SourceImage = null;
            CropPreview = null;

            CropX = 0;
            CropY = 0;
            CropWidth = 240;
            CropHeight = 240;

            ShowCropCommand = ConditionalCommand.From(CanShowCrop, ShowCrop);
            CenterCropCommand = ConditionalCommand.From(CanCenterCrop, CenterCrop);
        }

        // ------
        // Source
        // ------

        public BitmapSource? SourceImage { get; private set; }
        public bool HasSource => SourceImage != null;
        public int SourceWidth => SourceImage?.PixelWidth ?? 0;
        public int SourceHeight => SourceImage?.PixelHeight ?? 0;

        public void OnSourceChanged(BitmapSource? newImage)
        {
            SourceImage = newImage;
            if (CropWidth > SourceWidth || CropHeight > SourceHeight)
                ResizeTo(Math.Min(SourceWidth, SourceHeight));

            MoveTo((SourceWidth - CropWidth) / 2, (SourceHeight - CropHeight) / 2);
            UpdatePreview();
            CenterCrop();

            RaisePropertyChanged(nameof(SourceImage), nameof(HasSource), nameof(SourceWidth), nameof(SourceHeight));
            ShowCropCommand.RaiseCanExecuteChanged();
            CenterCropCommand.RaiseCanExecuteChanged();
        }

        // --------
        // Cropping
        // --------

        public CroppedBitmap? CropPreview { get; private set; }

        public void UpdatePreview()
        {
            CropPreview = SourceImage != null ? new CroppedBitmap(SourceImage, new Int32Rect(CropX, CropY, CropWidth, CropHeight)) : null;
            RaisePropertyChanged(nameof(CropPreview));
        }

        // --------
        // Position
        // --------

        public int CropX { get; private set; }
        public GridLength GridCropX => new GridLength(CropX);
        public int CropY { get; private set; }
        public GridLength GridCropY => new GridLength(CropY);

        public void MoveTo(int x, int y)
        {
            x = Math.Clamp(x, 0, SourceWidth - CropWidth);
            y = Math.Clamp(y, 0, SourceHeight - CropHeight);

            if (x == CropX && y == CropY)
                return;

            CropX = x;
            CropY = y;

            RaisePropertyChanged(
                nameof(CropX), nameof(GridCropX),
                nameof(CropY), nameof(GridCropY)
                );
        }

        // ----
        // Size
        // ----

        public int CropWidth { get; private set; }
        public GridLength GridCropWidth => new GridLength(CropWidth);
        public int CropHeight { get; private set; }
        public GridLength GridCropHeight => new GridLength(CropHeight);

        public string CropSizeString
        {
            get => CropWidth.ToString();
            set
            {
                if (!int.TryParse(value, out var newSize))
                    return;

                ResizeTo(newSize);
                UpdatePreview();
            }
        }

        public void ResizeTo(int size)
        {
            size = Math.Clamp(48, size, 960);
            if (size == CropWidth && size == CropHeight)
                return;

            var xshift = (CropWidth - size) / 2;
            var yshift = (CropHeight - size) / 2;
            CropWidth = size;
            CropHeight = size;

            MoveTo(CropX + xshift, CropY + yshift);

            RaisePropertyChanged(
                nameof(CropWidth), nameof(GridCropWidth),
                nameof(CropHeight), nameof(GridCropHeight),
                nameof(CropSizeString)
                );
        }

        // ---------
        // Scrolling
        // ---------

        public event EventHandler? ScrollToCropRequested;

        public IConditionalCommand ShowCropCommand { get; }
        private bool CanShowCrop() => HasSource;
        private void ShowCrop()
        {
            ScrollToCropRequested?.Invoke(this, EventArgs.Empty);
        }

        public IConditionalCommand CenterCropCommand { get; }
        private bool CanCenterCrop() => HasSource;
        private void CenterCrop()
        {
            MoveTo((SourceWidth - CropWidth) / 2, (SourceHeight - CropHeight) / 2);
            UpdatePreview();

            ScrollToCropRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
