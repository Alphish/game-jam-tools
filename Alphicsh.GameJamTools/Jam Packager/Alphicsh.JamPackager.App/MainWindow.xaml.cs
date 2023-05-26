using System.Windows;
using Alphicsh.JamPackager.ViewModel;

namespace Alphicsh.JamPackager.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new JamPackagerViewModel();
        }
    }
}
