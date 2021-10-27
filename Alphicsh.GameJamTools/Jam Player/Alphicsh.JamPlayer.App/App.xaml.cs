using System.Windows;
using System.Windows.Media.Imaging;

using Alphicsh.JamPlayer.Model;
using Alphicsh.JamPlayer.ViewModel;

namespace Alphicsh.JamPlayer.App
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
            LoadImageSourceResource("EntryPlaceholderSource", "Alphicsh.JamPlayer.App.Content.entry_placeholder.png");

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
    }
}