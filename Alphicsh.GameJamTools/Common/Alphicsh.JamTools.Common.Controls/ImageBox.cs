using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Alphicsh.JamTools.Common.Controls
{
    public class ImageBox : Border
    {
        public ImageBox()
        {
            this.SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.Fant);

            Background = new ImageBrush { Stretch = Stretch.UniformToFill };
        }

        private ImageBrush BackgroundBrush => (ImageBrush)this.Background;

        // ---------------------
        // Dependency properties
        // ---------------------

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source), typeof(ImageSource), typeof(ImageBox), new PropertyMetadata(null, OnSourceChange)
            );

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty PlaceholderSourceProperty = DependencyProperty.Register(
            nameof(PlaceholderSource), typeof(ImageSource), typeof(ImageBox), new PropertyMetadata(null, OnSourceChange)
            );

        public ImageSource PlaceholderSource
        {
            get => (ImageSource)GetValue(PlaceholderSourceProperty);
            set => SetValue(PlaceholderSourceProperty, value);
        }

        private static void OnSourceChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageBox = (ImageBox)d;
            imageBox.BackgroundBrush.ImageSource = imageBox.Source ?? imageBox.PlaceholderSource;
        }
    }
}
