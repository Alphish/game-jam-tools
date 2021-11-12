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
                rating.SetValue(InnerWidthProperty, rating.Width - 16d);
            else if (args.Property == InnerWidthProperty)
                rating.SetValue(WidthProperty, rating.InnerWidth + 16d);
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
