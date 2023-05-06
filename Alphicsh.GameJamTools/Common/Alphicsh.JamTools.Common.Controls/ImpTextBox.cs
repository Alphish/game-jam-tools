using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

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
                Keyboard.ClearFocus();
        }
    }
}
