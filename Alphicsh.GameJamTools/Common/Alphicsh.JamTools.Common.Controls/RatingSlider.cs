using System;
using System.Linq.Expressions;
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

        // ----------------
        // Properties utils
        // ----------------

        private static DependencyProperty RegisterProperty<TValue>(Expression<Func<RatingSlider, TValue>> propertyExpression)
        {
            return DependencyPropertyHelper.Register(propertyExpression);
        }

        private static DependencyProperty RegisterProperty<TValue>(
            Expression<Func<RatingSlider, TValue>> propertyExpression,
            TValue defaultValue
            )
        {
            return DependencyPropertyHelper.Register(propertyExpression, defaultValue);
        }

        private static DependencyProperty RegisterProperty<TValue>(
            Expression<Func<RatingSlider, TValue>> propertyExpression,
            TValue defaultValue,
            PropertyChangedCallback onPropertyChanged
            )
        {
            return DependencyPropertyHelper.Register(propertyExpression, defaultValue, onPropertyChanged);
        }

        private static DependencyPropertyKey RegisterReadOnlyProperty<TValue>(
            Expression<Func<RatingSlider, TValue>> propertyExpression,
            TValue defaultValue
            )
        {
            return DependencyPropertyHelper.RegisterReadOnly(propertyExpression, defaultValue);
        }

        // ----------------
        // Value properties
        // ----------------

        private static readonly DependencyPropertyKey OverValuePropertyKey = RegisterReadOnlyProperty(x => x.OverValue, 0d);
        public static DependencyProperty OverValueProperty => OverValuePropertyKey.DependencyProperty;
        public double OverValue => (double)GetValue(OverValueProperty);

        private static readonly DependencyPropertyKey UnderValuePropertyKey = RegisterReadOnlyProperty(x => x.UnderValue, 0d);
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

        public static readonly DependencyProperty TileSizeProperty = RegisterProperty(x => x.TileSize, new Size(24, 24));
        public Size TileSize
        {
            get => (Size)GetValue(TileSizeProperty);
            set => SetValue(TileSizeProperty, value);
        }

        public static readonly DependencyProperty InnerWidthProperty = RegisterProperty(x => x.InnerWidth, 0d, RecalculateWidth);
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

        public static readonly DependencyProperty ForegroundImageProperty = RegisterProperty(x => x.ForegroundImage, null, ResolveForeground);
        public ImageSource? ForegroundImage
        {
            get => (ImageSource?)GetValue(ForegroundImageProperty);
            set => SetValue(ForegroundImageProperty, value);
        }

        private static readonly DependencyPropertyKey ActualForegroundPropertyKey = RegisterReadOnlyProperty(x => x.ActualForeground, null);
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

        public static readonly DependencyProperty ForegroundMaskProperty
            = RegisterProperty(x => x.ForegroundMask, null, ResolveForegroundMask);
        public Brush? ForegroundMask
        {
            get => (Brush?)GetValue(ForegroundMaskProperty);
            set => SetValue(ForegroundMaskProperty, value);
        }

        public static readonly DependencyProperty ForegroundMaskImageProperty
            = RegisterProperty(x => x.ForegroundMaskImage, null, ResolveForegroundMask);
        public ImageSource? ForegroundMaskImage
        {
            get => (ImageSource?)GetValue(ForegroundMaskImageProperty);
            set => SetValue(ForegroundMaskImageProperty, value);
        }

        private static readonly DependencyPropertyKey ActualForegroundMaskPropertyKey = RegisterReadOnlyProperty(x => x.ActualForegroundMask, null);
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

        public static readonly DependencyProperty BackgroundImageProperty = RegisterProperty(x => x.BackgroundImage, null, ResolveBackground);
        public ImageSource? BackgroundImage
        {
            get => (ImageSource?)GetValue(BackgroundImageProperty);
            set => SetValue(BackgroundImageProperty, value);
        }

        public static readonly DependencyProperty HasValueProperty = RegisterProperty(x => x.HasValue, defaultValue: true, ResolveBackground);
        public bool HasValue
        {
            get => (bool)GetValue(HasValueProperty);
            set => SetValue(HasValueProperty, value);
        }

        public static readonly DependencyProperty NoValueBackgroundProperty = RegisterProperty(x => x.NoValueBackground, null, ResolveBackground);
        public Brush? NoValueBackground
        {
            get => (Brush?)GetValue(NoValueBackgroundProperty);
            set => SetValue(NoValueBackgroundProperty, value);
        }

        public static readonly DependencyProperty NoValueBackgroundImageProperty = RegisterProperty(x => x.NoValueBackgroundImage, null, ResolveBackground);
        public ImageSource? NoValueBackgroundImage
        {
            get => (ImageSource?)GetValue(NoValueBackgroundImageProperty);
            set => SetValue(NoValueBackgroundImageProperty, value);
        }

        private static readonly DependencyPropertyKey ActualBackgroundPropertyKey = RegisterReadOnlyProperty(x => x.ActualBackground, null);
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

        public static readonly DependencyProperty BackgroundMaskProperty
            = RegisterProperty(x => x.BackgroundMask, null, ResolveBackgroundMask);
        public Brush? BackgroundMask
        {
            get => (Brush?)GetValue(BackgroundMaskProperty);
            set => SetValue(BackgroundMaskProperty, value);
        }

        public static readonly DependencyProperty BackgroundMaskImageProperty
            = RegisterProperty(x => x.BackgroundMaskImage, null, ResolveBackgroundMask);
        public ImageSource? BackgroundMaskImage
        {
            get => (ImageSource?)GetValue(BackgroundMaskImageProperty);
            set => SetValue(BackgroundMaskImageProperty, value);
        }

        private static readonly DependencyPropertyKey ActualBackgroundMaskPropertyKey = RegisterReadOnlyProperty(x => x.ActualBackgroundMask, null);
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

        // -------------------
        // Thumb sliding setup
        // -------------------

        private Thumb? InnerThumb = null;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (InnerThumb != null)
                InnerThumb.MouseEnter -= OnThumbMouseEnter;

            InnerThumb = ((Track)GetTemplateChild("PART_Track")).Thumb;
            
            if (InnerThumb != null)
                InnerThumb.MouseEnter += OnThumbMouseEnter;
        }

        private void OnThumbMouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // if pressed mouse enters the thumb, the thumb is automatically grabbed
                MouseButtonEventArgs args = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left);
                args.RoutedEvent = MouseLeftButtonDownEvent;
                ((Thumb)sender).RaiseEvent(args);
            }
        }
    }
}
