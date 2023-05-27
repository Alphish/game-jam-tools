using System.Windows;
using Alphicsh.JamPackager.Model;
using Alphicsh.JamPackager.ViewModel;
using Alphicsh.JamPackager.Controls;
using Alphicsh.JamTools.Common.Theming;

namespace Alphicsh.JamPackager.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public JamPackagerViewModel ViewModel { get; }

        public App()
        {
            var model = new JamPackagerModel();
            ViewModel = new JamPackagerViewModel(model);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ThemeManager.Create(Resources);

            base.OnStartup(e);

            var window = new MainWindow() { DataContext = ViewModel };
            window.Show();
        }
    }
}
