using System.Windows;
using System.Windows.Media;

namespace Alphicsh.JamTools.Common.Theming
{
    public static class ButtonTheming
    {
        // HoverBrush
        public static readonly DependencyProperty HoverBrushProperty
            = DependencyProperty.RegisterAttached("HoverBrush", typeof(Brush), typeof(ButtonTheming));
        public static Brush GetHoverBrush(DependencyObject obj)
            => (Brush)obj.GetValue(HoverBrushProperty);
        public static void SetHoverBrush(DependencyObject obj, Brush value)
            => obj.SetValue(HoverBrushProperty, value);

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

        // DisabledForegroundBrush
        public static readonly DependencyProperty DisabledForegroundBrushProperty
            = DependencyProperty.RegisterAttached("DisabledForegroundBrush", typeof(Brush), typeof(ButtonTheming));
        public static Brush GetDisabledForegroundBrush(DependencyObject obj)
            => (Brush)obj.GetValue(DisabledForegroundBrushProperty);
        public static void SetDisabledForegroundBrush(DependencyObject obj, Brush value)
            => obj.SetValue(DisabledForegroundBrushProperty, value);

        // DisabledBackgroundBrush
        public static readonly DependencyProperty DisabledBackgrounBrushProperty
            = DependencyProperty.RegisterAttached("DisabledBackgroundBrush", typeof(Brush), typeof(ButtonTheming));
        public static Brush GetDisabledBackgroundBrush(DependencyObject obj)
            => (Brush)obj.GetValue(DisabledBackgrounBrushProperty);
        public static void SetDisabledBackgroundBrush(DependencyObject obj, Brush value)
            => obj.SetValue(DisabledBackgrounBrushProperty, value);

        // DisabledGlowBrush
        public static readonly DependencyProperty DisabledGlowBrushProperty
            = DependencyProperty.RegisterAttached("DisabledGlowBrush", typeof(Brush), typeof(ButtonTheming));
        public static Brush GetDisabledGlowBrush(DependencyObject obj)
            => (Brush)obj.GetValue(DisabledGlowBrushProperty);
        public static void SetDisabledGlowBrush(DependencyObject obj, Brush value)
            => obj.SetValue(DisabledGlowBrushProperty, value);

        // DisabledShadowBrush
        public static readonly DependencyProperty DisabledShadowBrushProperty
            = DependencyProperty.RegisterAttached("DisabledShadowBrush", typeof(Brush), typeof(ButtonTheming));
        public static Brush GetDisabledShadowBrush(DependencyObject obj)
            => (Brush)obj.GetValue(DisabledShadowBrushProperty);
        public static void SetDisabledShadowBrush(DependencyObject obj, Brush value)
            => obj.SetValue(DisabledShadowBrushProperty, value);
    }
}
