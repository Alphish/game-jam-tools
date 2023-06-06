using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamTools.Common.Mvvm.NotifiableProperties
{
    public class ImageSourceProperty<TViewModel> : NotifiableProperty
        where TViewModel : IViewModel
    {
        protected new TViewModel ViewModel => (TViewModel)base.ViewModel;

        private Func<TViewModel, FilePath?> PathGetter { get; }
        private Action<TViewModel, FilePath?>? PathSetter { get; }
        public bool IsReadonly => PathSetter == null;

        public ImageSourceProperty(
            TViewModel viewModel, string imagePropertyName,
            Func<TViewModel, FilePath?> pathGetter, Action<TViewModel, FilePath?>? pathSetter
            ) : base(viewModel, imagePropertyName)
        {
            PathGetter = pathGetter;
            PathSetter = pathSetter;

            RefreshSource();
        }

        // --------------
        // Exposing value
        // --------------

        public FilePath? Value
        {
            get => PathGetter(ViewModel);
            set
            {
                if (PathSetter == null)
                    throw new NotSupportedException("Cannot set a value of the readonly bitmap path property.");

                if (object.Equals(PathGetter(ViewModel), value))
                    return;

                PathSetter.Invoke(ViewModel, value);
                RaisePropertyChanged();
            }
        }

        public override void OnPropertyChange()
        {
            RefreshSource();
        }

        // --------------
        // Exposing image
        // --------------

        public ImageSource? ImageSource { get; private set; }

        private void RefreshSource()
        {
            var path = PathGetter(ViewModel);
            if (path == null || !path.Value.HasFile())
            {
                ImageSource = null;
                return;
            }

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bitmapImage.UriSource = path.Value.ToUri();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            ImageSource = bitmapImage;
        }
    }

    // ---------------
    // Static creation
    // ---------------

    public static class ImageSourceProperty
    {
        public static ImageSourceProperty<TViewModel> Create<TViewModel>(
            TViewModel viewModel, string imagePropertyName,
            Func<TViewModel, FilePath?> pathGetter, Action<TViewModel, FilePath?> pathSetter
            )
            where TViewModel : IViewModel
        {
            return new ImageSourceProperty<TViewModel>(viewModel, imagePropertyName, pathGetter, pathSetter);
        }

        public static ImageSourceProperty<TViewModel> CreateReadonly<TViewModel>(
            TViewModel viewModel, string imagePropertyName, Func<TViewModel, FilePath?> pathGetter
            )
            where TViewModel : IViewModel
        {
            return new ImageSourceProperty<TViewModel>(viewModel, imagePropertyName, pathGetter, pathSetter: null);
        }
    }
}
