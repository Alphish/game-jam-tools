using System.Linq;
using System.Windows;
using Alphicsh.EntryPackager.Controls;
using Alphicsh.EntryPackager.Model;
using Alphicsh.EntryPackager.ViewModel;
using Alphicsh.JamTools.Common.IO;
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
            OpenEntryFromArgs(e);

            base.OnStartup(e);

            var window = new MainWindow() { DataContext = ViewModel };
            window.Show();
        }

        private void OpenEntryFromArgs(StartupEventArgs e)
        {
            var directoryPath = FilePath.FromNullable(e.Args.FirstOrDefault());
            if (directoryPath.HasValue && directoryPath.Value.HasDirectory())
            {
                ViewModel.LoadEntryDirectory(directoryPath.Value);
            }
        }
    }
}
