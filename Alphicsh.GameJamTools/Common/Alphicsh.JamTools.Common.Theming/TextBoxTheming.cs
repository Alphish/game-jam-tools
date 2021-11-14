using System.Windows;
using System.Windows.Media;

namespace Alphicsh.JamTools.Common.Theming
{
    public static class TextBoxTheming
    {
        // ShadowBrush
        public static readonly DependencyProperty ShadowBrushProperty
            = DependencyProperty.RegisterAttached("ShadowBrush", typeof(Brush), typeof(TextBoxTheming));
        public static Brush GetShadowBrush(DependencyObject obj)
            => (Brush)obj.GetValue(ShadowBrushProperty);
        public static void SetShadowBrush(DependencyObject obj, Brush value)
            => obj.SetValue(ShadowBrushProperty, value);

        // GlowBrush
        public static readonly DependencyProperty GlowBrushProperty
            = DependencyProperty.RegisterAttached("GlowBrush", typeof(Brush), typeof(TextBoxTheming));
        public static Brush GetGlowBrush(DependencyObject obj)
            => (Brush)obj.GetValue(GlowBrushProperty);
        public static void SetGlowBrush(DependencyObject obj, Brush value)
            => obj.SetValue(GlowBrushProperty, value);

        // FocusBrush
        public static readonly DependencyProperty FocusBrushProperty
            = DependencyProperty.RegisterAttached("FocusBrush", typeof(Brush), typeof(TextBoxTheming));
        public static Brush GetFocusBrush(DependencyObject obj)
            => (Brush)obj.GetValue(FocusBrushProperty);
        public static void SetHoverBrush(DependencyObject obj, Brush value)
            => obj.SetValue(FocusBrushProperty, value);
    }
}
