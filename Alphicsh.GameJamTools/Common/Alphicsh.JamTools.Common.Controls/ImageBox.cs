using System;
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

        public static DependencyProperty SourceProperty { get; } = Deps.Register(x => x.Source, null, OnSourceChanged);
        public ImageSource? Source
        {
            get => (ImageSource?)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static DependencyProperty PlaceholderSourceProperty { get; }
            = Deps.Register(x => x.PlaceholderSource, null, OnSourceChanged);
        public ImageSource? PlaceholderSource
        {
            get => (ImageSource?)GetValue(PlaceholderSourceProperty);
            set => SetValue(PlaceholderSourceProperty, value);
        }

        private static DependencyPropertyKey ResolvedSourcePropertyKey { get; } = Deps.RegisterReadOnly(x => x.ResolvedSource, null);
        public static DependencyProperty ResolvedSourceProperty => ResolvedSourcePropertyKey.DependencyProperty;
        public ImageSource? ResolvedSource => (ImageSource?)GetValue(ResolvedSourceProperty);

        private static void OnSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var imageBox = (ImageBox)dependencyObject;
            imageBox.SetValue(ResolvedSourcePropertyKey, imageBox.Source ?? imageBox.PlaceholderSource);
        }

        // -----------------
        // Commands handling
        // -----------------

        public static DependencyProperty CommandProperty { get; } = Deps.Register(x => x.Command, null, OnCommandChanged);
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

        public static DependencyProperty HoverBrushProperty { get; } = Deps.Register(x => x.HoverBrush);
        public Brush? HoverBrush
        {
            get => (Brush?)GetValue(HoverBrushProperty);
            set => SetValue(HoverBrushProperty, value);
        }

        public static DependencyProperty DisabledBrushProperty { get; } = Deps.Register(x => x.DisabledBrush);
        public Brush? DisabledBrush
        {
            get => (Brush?)GetValue(DisabledBrushProperty);
            set => SetValue(DisabledBrushProperty, value);
        }

        private static void OnCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var imageBox = (ImageBox)dependencyObject;
            var oldCommand = args.OldValue as ICommand;
            var newCommand = args.NewValue as ICommand;

            if (oldCommand != null)
                oldCommand.CanExecuteChanged -= imageBox.OnCanExecuteChanged;
            if (newCommand != null)
                newCommand.CanExecuteChanged += imageBox.OnCanExecuteChanged;

            imageBox.IsEnabled = newCommand?.CanExecute(imageBox.CommandParameter) ?? true;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            Command?.Execute(CommandParameter);
        }

        protected void OnCanExecuteChanged(object? sender, EventArgs e)
        {
            IsEnabled = Command?.CanExecute(CommandParameter) ?? true;
        }
    }
}
