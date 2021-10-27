using System.Windows;

namespace Alphicsh.JamPlayer.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = App.Current.ViewModel;
        }
    }
}