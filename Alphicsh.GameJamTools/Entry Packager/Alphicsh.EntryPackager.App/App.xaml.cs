using System.Windows;
using Alphicsh.EntryPackager.Controls;
using Alphicsh.EntryPackager.Model;
using Alphicsh.EntryPackager.ViewModel;

namespace Alphicsh.EntryPackager.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public AppViewModel ViewModel { get; }

        public App()
        {
            var model = new AppModel();
            ViewModel = new AppViewModel(model);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var window = new MainWindow() { DataContext = ViewModel };
            window.Show();
        }
    }
}
