﻿using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Search;

using Alphicsh.JamPlayer.Model;
using Alphicsh.JamPlayer.ViewModel;
using Alphicsh.JamTools.Common.Theming;
using Alphicsh.JamPlayer.Controls;

namespace Alphicsh.JamPlayer.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static new App Current => (App)Application.Current;

        public AppViewModel ViewModel { get; }

        public App()
        {
            var model = new AppModel();
            ViewModel = new AppViewModel(model);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var themeManager = new ThemeManager(this.Resources);
            LoadJam(e);

            base.OnStartup(e);

            var window = new MainWindow();
            window.DataContext = ViewModel;
            window.Show();
        }

        private void LoadJam(StartupEventArgs e)
        {
            var jamFilePath = FilePath.FromNullable(e.Args.FirstOrDefault()) ?? GetJamInfoPathInAppDirectory();
            if (jamFilePath.HasValue)
                ViewModel.LoadJamFromFile(jamFilePath.Value);
        }

        private FilePath? GetJamInfoPathInAppDirectory()
        {
            var executableFileName = Process.GetCurrentProcess().MainModule!.FileName!;
            var jamPlayerPath = FilePath.From(executableFileName);
            var jamPlayerDirectory = jamPlayerPath.GetParentDirectoryPath()!.Value;

            var jamInfoPaths = FilesystemSearch.ForFilesIn(jamPlayerDirectory)
                .IncludingTopDirectoryOnly()
                .WithExtensions(".jaminfo")
                .FindAll()
                .FoundPaths;

            // not using FirstOrDefault()
            // because jamInfoPaths is a collection of non-nullable FilePaths
            return jamInfoPaths.Any() ? jamInfoPaths.First() : null;
        }
    }
}