using System.Windows;
using System.Windows.Controls;

namespace Alphicsh.JamTools.Common.Controls
{
    public class ImpButton : Button
    {
        private static readonly DependencyPropertyHelper<ImpButton> Deps
            = new DependencyPropertyHelper<ImpButton>();

        // ----------
        // Properties
        // ----------

        // Bar position

        public static readonly DependencyProperty BarPositionProperty
            = Deps.Register(x => x.BarPosition, BarPosition.Middle, RecalculateCornerRadius);
        public BarPosition BarPosition
        {
            get => (BarPosition)GetValue(BarPositionProperty);
            set => SetValue(BarPositionProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty
            = Deps.Register(x => x.CornerRadius, 0d, RecalculateCornerRadius);
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyPropertyKey ActualCornerRadiusPropertyKey
            = Deps.RegisterReadOnly(x => x.ActualCornerRadius, new CornerRadius());
        public static DependencyProperty ActualCornerRadiusProperty => ActualCornerRadiusPropertyKey.DependencyProperty;
        public CornerRadius ActualCornerRadius => (CornerRadius)GetValue(ActualCornerRadiusProperty);

        private static void RecalculateCornerRadius(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var button = (ImpButton)dependencyObject;
            button.SetValue(ActualCornerRadiusPropertyKey, button.ResolveCornerRadius());
        }

        private CornerRadius ResolveCornerRadius()
        {
            switch (BarPosition)
            {
                case BarPosition.Start:
                    return new CornerRadius(this.CornerRadius, 0d, 0d, this.CornerRadius);
                case BarPosition.Middle:
                    return new CornerRadius(0d, 0d, 0d, 0d);
                case BarPosition.End:
                    return new CornerRadius(0d, this.CornerRadius, this.CornerRadius, 0d);
                default:
                    return new CornerRadius(this.CornerRadius);
            }
        }
    }
}
