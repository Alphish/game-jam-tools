using System.Windows;
using Alphicsh.EntryPackager.Controls;
using Alphicsh.EntryPackager.Model;
using Alphicsh.EntryPackager.ViewModel;
using Alphicsh.JamTools.Common.Theming;

namespace Alphicsh.EntryPackager.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public EntryPackagerViewModel ViewModel { get; }

        public App()
        {
            ModalsRegistration.Register();

            var model = new AppModel();
            ViewModel = new EntryPackagerViewModel(model);
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
