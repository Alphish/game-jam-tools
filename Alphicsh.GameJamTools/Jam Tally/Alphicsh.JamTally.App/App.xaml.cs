using System.Windows;
using Alphicsh.JamTally.Controls;
using Alphicsh.JamTally.Model;
using Alphicsh.JamTally.ViewModel;
using Alphicsh.JamTools.Common.Theming;

namespace Alphicsh.JamTally.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public JamTallyViewModel ViewModel { get; }

        public App()
        {
            ModalsRegistration.Register();

            var model = new JamTallyModel();
            ViewModel = new JamTallyViewModel(model);
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
