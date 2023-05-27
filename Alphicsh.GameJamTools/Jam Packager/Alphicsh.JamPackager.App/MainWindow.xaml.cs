using System.Windows;
using Alphicsh.JamPackager.Model;
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

            var model = new JamPackagerModel();
            DataContext = new JamPackagerViewModel(model);
        }
    }
}
