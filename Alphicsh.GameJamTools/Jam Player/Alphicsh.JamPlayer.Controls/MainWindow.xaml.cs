using System.Windows;
using System.Windows.Input;

namespace Alphicsh.JamPlayer.Controls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
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
