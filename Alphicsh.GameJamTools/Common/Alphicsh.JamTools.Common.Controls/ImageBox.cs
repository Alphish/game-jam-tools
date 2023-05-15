using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Alphicsh.JamTools.Common.Controls
{
    public class ImageBox : Border
    {
        private static DependencyPropertyHelper<ImageBox> Deps { get; } = new DependencyPropertyHelper<ImageBox>();

        // ---------------
        // Source handling
        // ---------------

        public static DependencyProperty SourceProperty { get; } = Deps.Register(x => x.Source, null, OnSourceChange);
        public ImageSource? Source
        {
            get => (ImageSource?)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static DependencyProperty PlaceholderSourceProperty { get; }
            = Deps.Register(x => x.PlaceholderSource, null, OnSourceChange);
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

        // -----------------
        // Commands handling
        // -----------------

        public static DependencyProperty CommandProperty { get; } = Deps.Register(x => x.Command);
        public ICommand? Command
        {
            get => (ICommand?)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static DependencyProperty CommandParameterProperty { get; } = Deps.Register(x => x.CommandParameter);
        public object? CommandParameter
        {
            get => (object?)GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            Command?.Execute(CommandParameter);
        }
    }
}
