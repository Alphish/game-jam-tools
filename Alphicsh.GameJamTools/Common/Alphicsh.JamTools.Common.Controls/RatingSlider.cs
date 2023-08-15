using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Alphicsh.JamTools.Common.Controls
{
    public class RatingSlider : Slider
    {
        static RatingSlider()
        {
            ValueProperty.OverrideMetadata(typeof(RatingSlider), new FrameworkPropertyMetadata(defaultValue: 0d, RecalculateRelativeValues));
            MinimumProperty.OverrideMetadata(typeof(RatingSlider), new FrameworkPropertyMetadata(defaultValue: 0d, RecalculateRelativeValues));
            MaximumProperty.OverrideMetadata(typeof(RatingSlider), new FrameworkPropertyMetadata(defaultValue: 10d, RecalculateRelativeValues));

            WidthProperty.OverrideMetadata(typeof(RatingSlider), new FrameworkPropertyMetadata(defaultValue: 0d, RecalculateWidth));

            ForegroundProperty.OverrideMetadata(typeof(RatingSlider), new FrameworkPropertyMetadata(defaultValue: null, ResolveForeground));
            BackgroundProperty.OverrideMetadata(typeof(RatingSlider), new FrameworkPropertyMetadata(defaultValue: null, ResolveBackground));
        }

        private static DependencyPropertyHelper<RatingSlider> Deps
            = new DependencyPropertyHelper<RatingSlider>();

        public RatingSlider()
        {
            IsMoveToPointEnabled = true;
        }

        // ----------------
        // Value properties
        // ----------------

        private static readonly DependencyPropertyKey OverValuePropertyKey = Deps.RegisterReadOnly(x => x.OverValue, 0d);
        public static DependencyProperty OverValueProperty => OverValuePropertyKey.DependencyProperty;
        public double OverValue => (double)GetValue(OverValueProperty);

        private static readonly DependencyPropertyKey UnderValuePropertyKey = Deps.RegisterReadOnly(x => x.UnderValue, 0d);
        public static DependencyProperty UnderValueProperty => UnderValuePropertyKey.DependencyProperty;
        public double UnderValue => (double)GetValue(UnderValueProperty);

        private static void RecalculateRelativeValues(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var rating = (RatingSlider)dependencyObject;
            rating.SetValue(OverValuePropertyKey, rating.Value - rating.Minimum);
            rating.SetValue(UnderValuePropertyKey, rating.Maximum - rating.Value);
        }

        // ---------------
        // Size properties
        // ---------------

        public static readonly DependencyProperty TileSizeProperty = Deps.Register(x => x.TileSize, new Size(24, 24));
        public Size TileSize
        {
            get => (Size)GetValue(TileSizeProperty);
            set => SetValue(TileSizeProperty, value);
        }

        public static readonly DependencyProperty InnerWidthProperty = Deps.Register(x => x.InnerWidth, 0d, RecalculateWidth);
        public double InnerWidth
        {
            get => (double)GetValue(InnerWidthProperty);
            set => SetValue(InnerWidthProperty, value);
        }

        private static void RecalculateWidth(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var rating = (RatingSlider)dependencyObject;
            if (args.Property == WidthProperty)
                rating.SetValue(InnerWidthProperty, rating.Width - 8d);
            else if (args.Property == InnerWidthProperty)
                rating.SetValue(WidthProperty, rating.InnerWidth + 8d);
        }

        // ----------------
        // Foreground brush
        // ----------------

        public static readonly DependencyProperty ForegroundImageProperty = Deps.Register(x => x.ForegroundImage, null, ResolveForeground);
        public ImageSource? ForegroundImage
        {
            get => (ImageSource?)GetValue(ForegroundImageProperty);
            set => SetValue(ForegroundImageProperty, value);
        }

        private static readonly DependencyPropertyKey ActualForegroundPropertyKey = Deps.RegisterReadOnly(x => x.ActualForeground, null);
        public static DependencyProperty ActualForegroundProperty => ActualForegroundPropertyKey.DependencyProperty;
        public Brush? ActualForeground => (Brush)GetValue(ActualForegroundProperty);

        private static void ResolveForeground(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var rating = (RatingSlider)dependencyObject;
            var brush = ResolveBrush(rating.ForegroundImage, rating.TileSize, rating.Foreground);
            rating.SetValue(ActualForegroundPropertyKey, brush);
        }

        // ---------------
        // Foreground mask
        // ---------------

        public static readonly DependencyProperty ForegroundMaskProperty = Deps.Register(x => x.ForegroundMask, null, ResolveForegroundMask);
        public Brush? ForegroundMask
        {
            get => (Brush?)GetValue(ForegroundMaskProperty);
            set => SetValue(ForegroundMaskProperty, value);
        }

        public static readonly DependencyProperty ForegroundMaskImageProperty = Deps.Register(x => x.ForegroundMaskImage, null, ResolveForegroundMask);
        public ImageSource? ForegroundMaskImage
        {
            get => (ImageSource?)GetValue(ForegroundMaskImageProperty);
            set => SetValue(ForegroundMaskImageProperty, value);
        }

        private static readonly DependencyPropertyKey ActualForegroundMaskPropertyKey = Deps.RegisterReadOnly(x => x.ActualForegroundMask, null);
        public static DependencyProperty ActualForegroundMaskProperty => ActualForegroundMaskPropertyKey.DependencyProperty;
        public Brush? ActualForegroundMask => (Brush)GetValue(ActualForegroundMaskProperty);

        private static void ResolveForegroundMask(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var rating = (RatingSlider)dependencyObject;
            var brush = ResolveBrush(rating.ForegroundMaskImage, rating.TileSize, rating.ForegroundMask);
            rating.SetValue(ActualForegroundMaskPropertyKey, brush);
        }

        // ----------------
        // Background brush
        // ----------------

        public static readonly DependencyProperty BackgroundImageProperty = Deps.Register(x => x.BackgroundImage, null, ResolveBackground);
        public ImageSource? BackgroundImage
        {
            get => (ImageSource?)GetValue(BackgroundImageProperty);
            set => SetValue(BackgroundImageProperty, value);
        }

        public static readonly DependencyProperty HasValueProperty = Deps.Register(x => x.HasValue, defaultValue: true, ResolveBackground);
        public bool HasValue
        {
            get => (bool)GetValue(HasValueProperty);
            set => SetValue(HasValueProperty, value);
        }

        public static readonly DependencyProperty NoValueBackgroundProperty = Deps.Register(x => x.NoValueBackground, null, ResolveBackground);
        public Brush? NoValueBackground
        {
            get => (Brush?)GetValue(NoValueBackgroundProperty);
            set => SetValue(NoValueBackgroundProperty, value);
        }

        public static readonly DependencyProperty NoValueBackgroundImageProperty = Deps.Register(x => x.NoValueBackgroundImage, null, ResolveBackground);
        public ImageSource? NoValueBackgroundImage
        {
            get => (ImageSource?)GetValue(NoValueBackgroundImageProperty);
            set => SetValue(NoValueBackgroundImageProperty, value);
        }

        private static readonly DependencyPropertyKey ActualBackgroundPropertyKey = Deps.RegisterReadOnly(x => x.ActualBackground, null);
        public static DependencyProperty ActualBackgroundProperty => ActualBackgroundPropertyKey.DependencyProperty;
        public Brush? ActualBackground => (Brush)GetValue(ActualBackgroundProperty);

        private static void ResolveBackground(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var rating = (RatingSlider)dependencyObject;

            var backgroundImage = rating.HasValue || rating.NoValueBackgroundImage == null
               ? rating.BackgroundImage
               : rating.NoValueBackgroundImage;
            var plainBrush = rating.HasValue || rating.NoValueBackground == null ? rating.Background : rating.NoValueBackground;

            var brush = ResolveBrush(backgroundImage, rating.TileSize, plainBrush);
            rating.SetValue(ActualBackgroundPropertyKey, brush);
        }

        // ---------------
        // Background mask
        // ---------------

        public static readonly DependencyProperty BackgroundMaskProperty = Deps.Register(x => x.BackgroundMask, null, ResolveBackgroundMask);
        public Brush? BackgroundMask
        {
            get => (Brush?)GetValue(BackgroundMaskProperty);
            set => SetValue(BackgroundMaskProperty, value);
        }

        public static readonly DependencyProperty BackgroundMaskImageProperty = Deps.Register(x => x.BackgroundMaskImage, null, ResolveBackgroundMask);
        public ImageSource? BackgroundMaskImage
        {
            get => (ImageSource?)GetValue(BackgroundMaskImageProperty);
            set => SetValue(BackgroundMaskImageProperty, value);
        }

        private static readonly DependencyPropertyKey ActualBackgroundMaskPropertyKey = Deps.RegisterReadOnly(x => x.ActualBackgroundMask, null);
        public static DependencyProperty ActualBackgroundMaskProperty => ActualBackgroundMaskPropertyKey.DependencyProperty;
        public Brush? ActualBackgroundMask => (Brush)GetValue(ActualBackgroundMaskProperty);

        private static void ResolveBackgroundMask(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var rating = (RatingSlider)dependencyObject;
            var brush = ResolveBrush(rating.BackgroundMaskImage, rating.TileSize, rating.BackgroundMask);
            rating.SetValue(ActualBackgroundMaskPropertyKey, brush);
        }

        // -------------
        // Miscellaneous
        // -------------

        private static Brush? ResolveBrush(ImageSource? imageSource, Size tileSize, Brush? brush)
        {
            if (imageSource != null)
            {
                return new ImageBrush
                {
                    ImageSource = imageSource,
                    TileMode = TileMode.Tile,
                    Viewport = new Rect { X = 0, Y = 0, Size = tileSize },
                    ViewportUnits = BrushMappingMode.Absolute,
                };
            }
            else
            {
                return brush;
            }
        }

        // -----------------------
        // Sliding behaviour setup
        // -----------------------

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            MoveValueToMousePosition(e);
            CaptureMouse();

            var thumb = ((Track)GetTemplateChild("PART_Track")).Thumb;
            thumb.Focus();
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            if (!IsMouseCaptured)
                return;

            MoveValueToMousePosition(e);
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            Mouse.Capture(null);
        }

        private void MoveValueToMousePosition(MouseEventArgs e)
        {
            var position = e.GetPosition(this);

            var innerX = position.X - (ActualWidth - InnerWidth) / 2;
            var positionValue = Math.Ceiling(Maximum * innerX / InnerWidth);
            positionValue = Math.Clamp(positionValue, Minimum, Maximum);

            Value = positionValue;
        }
    }
}
