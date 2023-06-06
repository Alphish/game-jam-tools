using System;
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

            var model = new EntryPackagerModel();
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
            var argPath = FilePath.FromNullable(e.Args.FirstOrDefault());
            if (!argPath.HasValue)
                return;
            else if (argPath.Value.HasDirectory())
                ViewModel.LoadEntryDirectory(argPath.Value);
            else if (!argPath.Value.HasFile())
                return;
            else if (StringComparer.OrdinalIgnoreCase.Equals(argPath.Value.GetLastSegmentName(), "entry.jamentry"))
                ViewModel.LoadEntryInfo(argPath.Value);
            else if (StringComparer.OrdinalIgnoreCase.Equals(argPath.Value.GetExtension(), ".zip"))
                ViewModel.LoadEntryZip(argPath.Value);
        }
    }
}
