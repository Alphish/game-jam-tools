using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Alphicsh.JamPlayer.Controls;
using Alphicsh.JamPlayer.Model;
using Alphicsh.JamPlayer.ViewModel;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Search;
using Alphicsh.JamTools.Common.Theming;

namespace Alphicsh.JamPlayer.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static new App Current => (App)Application.Current;

        public JamPlayerViewModel ViewModel { get; }

        public App()
        {
            ModalsRegistration.Register();

            var model = new AppModel();
            ViewModel = JamPlayerViewModel.Create(model);
            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            ThemeManager.Create(Resources);
            await LoadJam(e);

            base.OnStartup(e);

            var window = new MainWindow
            {
                DataContext = ViewModel,
                WindowState = WindowState.Maximized,
            };
            window.Show();
        }

        private async Task LoadJam(StartupEventArgs e)
        {
            var jamFilePath = FilePath.FromNullable(e.Args.FirstOrDefault()) ?? GetJamInfoPathInAppDirectory();
            if (jamFilePath.HasValue)
                await ViewModel.LoadJamFromFile(jamFilePath.Value);
        }

        private FilePath? GetJamInfoPathInAppDirectory()
        {
            var executableFileName = Process.GetCurrentProcess().MainModule!.FileName!;
            var jamPlayerPath = FilePath.From(executableFileName);
            var jamPlayerDirectory = jamPlayerPath.GetParentDirectoryPath();

            var jamInfoPaths = FilesystemSearch.ForFilesIn(jamPlayerDirectory)
                .IncludingTopDirectoryOnly()
                .WithExtensions(".jaminfo")
                .FindAll()
                .FoundPaths;

            // not using FirstOrDefault()
            // because jamInfoPaths is a collection of non-nullable FilePaths
            // so it would return a default FilePath instead
            return jamInfoPaths.Any() ? jamInfoPaths.First() : null;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (ViewModel.Jam == null)
                return;

            var filePath = ViewModel.Jam.Model.DirectoryPath
                .Append(".crashes")
                .Append(DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt")
                .Value;

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            File.WriteAllText(filePath, e.Exception.ToString());
        }
    }
}