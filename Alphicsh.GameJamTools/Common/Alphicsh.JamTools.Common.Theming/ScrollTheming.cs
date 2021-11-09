using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Alphicsh.JamTools.Common.Theming
{
    public static class ScrollTheming
    {
        // ScrollBrush
        public static readonly DependencyProperty ScrollBrushProperty
            = DependencyProperty.RegisterAttached("ScrollBrush", typeof(Brush), typeof(ScrollTheming));
        public static Brush GetScrollBrush(DependencyObject obj)
            => (Brush)obj.GetValue(ScrollBrushProperty);
        public static void SetScrollBrush(DependencyObject obj, Brush value)
            => obj.SetValue(ScrollBrushProperty, value);

        // ScrollPressedBrush
        public static readonly DependencyProperty ScrollPressedBrushProperty
            = DependencyProperty.RegisterAttached("ScrollPressedBrush", typeof(Brush), typeof(ScrollTheming));
        public static Brush GetScrollPressedBrush(DependencyObject obj)
            => (Brush)obj.GetValue(ScrollPressedBrushProperty);
        public static void SetScrollPressedBrush(DependencyObject obj, Brush value)
            => obj.SetValue(ScrollPressedBrushProperty, value);

        // ScrollDisabledBrush
        public static readonly DependencyProperty ScrollDisabledBrushProperty
            = DependencyProperty.RegisterAttached("ScrollDisabledBrush", typeof(Brush), typeof(ScrollTheming));
        public static Brush GetScrollDisabledBrush(DependencyObject obj)
            => (Brush)obj.GetValue(ScrollDisabledBrushProperty);
        public static void SetScrollDisabledBrush(DependencyObject obj, Brush value)
            => obj.SetValue(ScrollDisabledBrushProperty, value);
    }
}
