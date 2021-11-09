using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Search;

using Alphicsh.JamPlayer.Model;
using Alphicsh.JamPlayer.ViewModel;

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
            LoadImageSourceResource("EntryPlaceholderSource", "Alphicsh.JamPlayer.App.Content.entry_placeholder.png");

            LoadJam(e);

            base.OnStartup(e);
        }

        private void LoadImageSourceResource(string resourceKey, string resourceName)
        {
            var assembly = typeof(MainWindow).Assembly;
            using var resourceStream = assembly.GetManifestResourceStream(resourceName);

            var imageSource = new BitmapImage();
            imageSource.BeginInit();
            imageSource.StreamSource = resourceStream;
            imageSource.CacheOption = BitmapCacheOption.OnLoad;
            imageSource.EndInit();

            Resources[resourceKey] = imageSource;
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

        private void LoadJam(StartupEventArgs e)
        {
            var jamFilePath = FilePath.FromNullable(e.Args.FirstOrDefault()) ?? GetJamInfoPathInAppDirectory();
            if (jamFilePath.HasValue)
                ViewModel.LoadJamFromFile(jamFilePath.Value);
        }
    }
}