using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

using Alphicsh.JamPlayer.Model;
using Alphicsh.JamPlayer.Model.Jam;
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

            InitData();

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

        private void InitData()
        {
            var jamModel = new JamOverview()
            {
                Entries = new List<JamEntry>
                {
                    CreateJamEntry("Another Winning Entry", teamName: null, "TehPilot", "TheFugue"),
                    CreateJamEntry("Cthildha", teamName: null, "Jordan Thomas", "Amber Thomas"),
                    CreateJamEntry("Escape of the Clowns", "Firehammer Games", "kburkhart84"),
                    CreateJamEntry("Evanski's Raccoon Adventure", teamName: null, "EvanSki"),
                    CreateJamEntry("Forty; or the Modern Big Brother", teamName: null, "Ezra"),
                    CreateJamEntry("Mansion Mayhem", "Team 70s", "Alice", "HayManMarc", "Micah_DS"),
                    CreateJamEntry("The Insufferable Pan", teamName: "Pixel-Team", "Pat Ferguson"),
                    CreateJamEntry("Ullr", teamName: null, "Siolfor the Jackal"),
                }
            };

            ViewModel.LoadJam(jamModel);
        }

        private JamEntry CreateJamEntry(string title, string? teamName, params string[] authorNames)
        {
            var authors = authorNames.Select(name => new JamAuthor { Name = name }).ToList();
            var team = new JamTeam { Name = teamName, Authors = authors };
            return new JamEntry { Title = title, Team = team };
        }
    }
}