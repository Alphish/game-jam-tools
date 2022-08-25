using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Alphicsh.JamTools.Common.Controls
{
    public class ImageBox : Border
    {
        private static DependencyPropertyHelper<ImageBox> Deps { get; } = new DependencyPropertyHelper<ImageBox>();

        // ---------------------
        // Dependency properties
        // ---------------------

        public static DependencyProperty SourceProperty { get; } = Deps.Register(control => control.Source, null, OnSourceChange);
        public ImageSource? Source
        {
            get => (ImageSource?)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static DependencyProperty PlaceholderSourceProperty { get; }
            = Deps.Register(control => control.PlaceholderSource, null, OnSourceChange);
        public ImageSource? PlaceholderSource
        {
            get => (ImageSource?)GetValue(PlaceholderSourceProperty);
            set => SetValue(PlaceholderSourceProperty, value);
        }

        private static DependencyPropertyKey ResolvedSourcePropertyKey { get; } = Deps.RegisterReadOnly(x => x.ResolvedSource, null);
        public static DependencyProperty ResolvedSourceProperty => ResolvedSourcePropertyKey.DependencyProperty;
        public ImageSource? ResolvedSource => (ImageSource?)GetValue(ResolvedSourceProperty);

        private static void OnSourceChange(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var imageBox = (ImageBox)dependencyObject;
            imageBox.SetValue(ResolvedSourcePropertyKey, imageBox.Source ?? imageBox.PlaceholderSource);
        }
    }
}
