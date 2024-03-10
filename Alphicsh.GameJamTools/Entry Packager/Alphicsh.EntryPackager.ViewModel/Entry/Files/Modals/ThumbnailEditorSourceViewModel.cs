using System.Drawing;
using System;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Alphicsh.JamTools.Common.Controls.Files;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Files.Modals
{
    public class ThumbnailEditorSourceViewModel : BaseViewModel
    {
        public ThumbnailEditorSourceViewModel(FilePath entryDirectory, ThumbnailEditorCropViewModel cropViewModel)
        {
            EntryDirectory = entryDirectory;
            CropViewModel = cropViewModel;

            LoadCommand = SimpleCommand.From(Load);
            PasteCommand = SimpleCommand.From(Paste);
        }

        private FilePath EntryDirectory { get; }
        private ThumbnailEditorCropViewModel CropViewModel { get; }

        // ------
        // Source
        // ------

        public BitmapSource? SourceImage => CropViewModel.SourceImage;
        private void SetSourceImage(BitmapSource? source)
        {
            if (source == null)
                return;

            if (source.PixelWidth < 48 || source.PixelHeight < 48)
                return;

            CropViewModel.OnSourceChanged(source);
            RaisePropertyChanged(nameof(SourceImage));
        }

        // -----------------
        // Loading from file
        // -----------------

        public ICommand LoadCommand { get; }
        private void Load()
        {
            var path = FileQuery.OpenFile()
                .FromDirectory(EntryDirectory)
                .WithFileType("*.png;*.jpg;*.jpeg;*.bmp", "Image files")
                .GetPath();

            if (path == null)
                return;

            var foundPath = path.Value;
            var image = LoadImageFromFile(foundPath);
            SetSourceImage(image);
        }

        private BitmapSource LoadImageFromFile(FilePath path)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bitmapImage.UriSource = path.ToUri();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            return bitmapImage;
        }

        // ----------------------
        // Pasting from clipboard
        // ----------------------

        public ICommand PasteCommand { get; }
        private void Paste()
        {
            var image = GetClipboardImage();
            SetSourceImage(image);
        }

        // Clipboard.GetImage() doesn't work properly for some images (e.g. copied from MS Paint)
        // so I used this Stack Overflow answer: https://stackoverflow.com/a/25751020
        // to cover more image Copy-Paste use cases

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static BitmapSource? GetClipboardImage()
        {
            var clipboardData = Clipboard.GetDataObject();
            if (clipboardData == null || !clipboardData.GetDataPresent(DataFormats.Bitmap))
                return null;

            var bitmap = (Bitmap)clipboardData.GetData(DataFormats.Bitmap);
            var hbitmap = bitmap.GetHbitmap();
            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hbitmap, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()
                    );
            }
            finally
            {
                DeleteObject(hbitmap);
            }
        }
    }
}
