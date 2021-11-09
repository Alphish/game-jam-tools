using System.Windows;
using System.Windows.Media;

namespace Alphicsh.JamTools.Common.Theming
{
    public static class ButtonTheming
    {
        // ShadowBrush
        public static readonly DependencyProperty ShadowBrushProperty
            = DependencyProperty.RegisterAttached("ShadowBrush", typeof(Brush), typeof(ButtonTheming));
        public static Brush GetShadowBrush(DependencyObject obj)
            => (Brush)obj.GetValue(ShadowBrushProperty);
        public static void SetShadowBrush(DependencyObject obj, Brush value)
            => obj.SetValue(ShadowBrushProperty, value);

        // GlowBrush
        public static readonly DependencyProperty GlowBrushProperty
            = DependencyProperty.RegisterAttached("GlowBrush", typeof(Brush), typeof(ButtonTheming));
        public static Brush GetGlowBrush(DependencyObject obj)
            => (Brush)obj.GetValue(GlowBrushProperty);
        public static void SetGlowBrush(DependencyObject obj, Brush value)
            => obj.SetValue(GlowBrushProperty, value);

        // HoverBrush
        public static readonly DependencyProperty HoverBrushProperty
            = DependencyProperty.RegisterAttached("HoverBrush", typeof(Brush), typeof(ButtonTheming));
        public static Brush GetHoverBrush(DependencyObject obj)
            => (Brush)obj.GetValue(HoverBrushProperty);
        public static void SetHoverBrush(DependencyObject obj, Brush value)
            => obj.SetValue(HoverBrushProperty, value);
    }
}
