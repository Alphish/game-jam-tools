using System;
using System.Windows;
using System.Windows.Input;

namespace Alphicsh.JamTools.Common.Mvvm
{
    public class AppWindow : Window
    {
        public static AppWindow Current { get; private set; } = null!;

        public AppWindow()
        {
            if (Current != null)
                throw new InvalidOperationException("AppWindow should be created only once.");

            Current = this;
        }

        public void ClearFocus()
        {
            var focusedElement = FocusManager.GetFocusedElement(this);
            focusedElement?.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
            FocusManager.SetFocusedElement(this, this);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            ClearFocus();
        }
    }
}
