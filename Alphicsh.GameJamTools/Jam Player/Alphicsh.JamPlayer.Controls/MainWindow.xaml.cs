using System;
using System.Windows;
using System.Windows.Input;

namespace Alphicsh.JamPlayer.Controls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Current { get; private set; } = null!;

        public MainWindow()
        {
            if (Current != null)
                throw new InvalidOperationException("MainWindow should be created only once.");

            Current = this;
            InitializeComponent();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var focusedElement = FocusManager.GetFocusedElement(this);
            focusedElement?.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
            FocusManager.SetFocusedElement(this, this);
        }
    }
}
