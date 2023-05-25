using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Alphicsh.JamTools.Common.Mvvm;

namespace Alphicsh.JamTools.Common.Controls
{
    public class ImpTextBox : TextBox
    {
        static ImpTextBox()
        {
            TextProperty.OverrideMetadata(
                typeof(ImpTextBox),
                new FrameworkPropertyMetadata(defaultValue: "", RecalculatePlaceholderVisibility)
                );
        }

        private static readonly DependencyPropertyHelper<ImpTextBox> Deps
            = new DependencyPropertyHelper<ImpTextBox>();

        // ----------
        // Properties
        // ----------

        // Placeholders

        public static readonly DependencyProperty PlaceholderProperty
            = Deps.Register(x => x.Placeholder, "", RecalculatePlaceholderVisibility);
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly DependencyProperty PlaceholderForegroundProperty = Deps.Register(x => x.PlaceholderForeground);
        public Brush PlaceholderForeground
        {
            get => (Brush)GetValue(PlaceholderForegroundProperty);
            set => SetValue(PlaceholderForegroundProperty, value);
        }

        public static readonly DependencyPropertyKey PlaceholderVisibilityPropertyKey
            = Deps.RegisterReadOnly(x => x.PlaceholderVisibility, Visibility.Visible);
        public static DependencyProperty PlaceholderVisibilityProperty => PlaceholderVisibilityPropertyKey.DependencyProperty;
        public Visibility PlaceholderVisibility => (Visibility)GetValue(PlaceholderVisibilityProperty);

        private static void RecalculatePlaceholderVisibility(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var textBox = (ImpTextBox)dependencyObject;
            var shouldShowPlaceholder = !string.IsNullOrWhiteSpace(textBox.Placeholder) && string.IsNullOrEmpty(textBox.Text);
            textBox.SetValue(PlaceholderVisibilityPropertyKey, shouldShowPlaceholder ? Visibility.Visible : Visibility.Collapsed);
        }

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
            var textBox = (ImpTextBox)dependencyObject;
            textBox.SetValue(ActualCornerRadiusPropertyKey, textBox.ResolveCornerRadius());
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

        // ---------
        // Overrides
        // ---------

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            var isSpecialEnter = e.Key == Key.Enter && !AcceptsReturn;
            var isSpecialEsc = e.Key == Key.Escape;

            if (isSpecialEnter || isSpecialEsc)
            {
                var binding = BindingOperations.GetBindingExpression(this, TextBox.TextProperty);
                binding?.UpdateSource();
            }

            if (isSpecialEsc)
                AppWindow.Current.ClearFocus();
        }
    }
}
