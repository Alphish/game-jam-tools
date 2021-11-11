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
        }

        // ----------------
        // Value properties
        // ----------------

        private static readonly DependencyPropertyKey OverValuePropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(OverValue), typeof(double), typeof(RatingSlider), new PropertyMetadata(defaultValue: 0d)
            );

        public static DependencyProperty OverValueProperty
            => OverValuePropertyKey.DependencyProperty;

        public double OverValue
        {
            get => (double)GetValue(OverValueProperty);
            set => SetValue(ValueProperty, (double)GetValue(MinimumProperty) + value);
        }

        private static readonly DependencyPropertyKey UnderValuePropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(UnderValue), typeof(double), typeof(RatingSlider), new PropertyMetadata(defaultValue: 0d)
            );

        public static DependencyProperty UnderValueProperty
            => UnderValuePropertyKey.DependencyProperty;

        public double UnderValue
        {
            get => (double)GetValue(UnderValueProperty);
            set => SetValue(ValueProperty, (double)GetValue(MaximumProperty) - value);
        }

        private static void RecalculateRelativeValues(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var rating = (RatingSlider)dependencyObject;
            var currentValue = (double)rating.GetValue(ValueProperty);

            rating.SetValue(OverValuePropertyKey, currentValue - (double)rating.GetValue(MinimumProperty));
            rating.SetValue(UnderValuePropertyKey, (double)rating.GetValue(MaximumProperty) - currentValue);
        }

        // ---------------
        // Size properties
        // ---------------

        public static readonly DependencyProperty InnerWidthProperty = DependencyProperty.Register(
            nameof(InnerWidth), typeof(double), typeof(RatingSlider), new PropertyMetadata(defaultValue: 0d, RecalculateWidth)
            );

        public double InnerWidth
        {
            get => (double)GetValue(InnerWidthProperty);
            set => SetValue(InnerWidthProperty, value);
        }

        private static void RecalculateWidth(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var rating = (RatingSlider)dependencyObject;
            if (args.Property == WidthProperty)
                rating.SetValue(InnerWidthProperty, (double)rating.GetValue(WidthProperty) - 16d);
            else if (args.Property == InnerWidthProperty)
                rating.SetValue(WidthProperty, (double)rating.GetValue(InnerWidthProperty) + 16d);
        }

        // ------------------
        // Opacity properties
        // ------------------

        public static readonly DependencyProperty ForegroundMaskProperty = DependencyProperty.Register(
            nameof(ForegroundMask), typeof(Brush), typeof(RatingSlider)
            );

        public Brush ForegroundMask
        {
            get => (Brush)GetValue(ForegroundMaskProperty);
            set => SetValue(ForegroundMaskProperty, value);
        }

        public static readonly DependencyProperty BackgroundMaskProperty = DependencyProperty.Register(
            nameof(BackgroundMask), typeof(Brush), typeof(RatingSlider)
            );

        public Brush BackgroundMask
        {
            get => (Brush)GetValue(BackgroundMaskProperty);
            set => SetValue(BackgroundMaskProperty, value);
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
