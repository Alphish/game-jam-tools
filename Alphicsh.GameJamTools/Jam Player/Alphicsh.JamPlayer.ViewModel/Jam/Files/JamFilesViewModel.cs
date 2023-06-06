using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Alphicsh.JamPlayer.Model.Jam.Files;
using Alphicsh.JamPlayer.ViewModel.Jam.Files.Modals;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.JamPlayer.ViewModel.Jam.Files
{
    public class JamFilesViewModel : WrapperViewModel<JamFiles>
    {
        private static ProcessLauncher ProcessLauncher { get; } = new ProcessLauncher();
        private static EntryLauncher EntryLauncher { get; } = new EntryLauncher();

        public JamFilesViewModel(JamFiles model) : base(model)
        {
            Launchers = Model.Launchers;
            PlayCommand = SimpleCommand.From(Play);

            Readme = model.Readme != null ? new JamReadmeViewModel(model.Readme) : null;
            Afterword = model.Afterword != null ? new JamAfterwordViewModel(model.Afterword) : null;
            Thumbnails = model.Thumbnails != null ? new JamThumbnailsViewModel(model.Thumbnails) : null;

            OpenDirectoryCommand = SimpleCommand.From(OpenDirectory);
        }

        // ---------
        // Launchers
        // ---------

        public IReadOnlyCollection<LaunchData> Launchers { get; }
        public string PlayDescription
        {
            get
            {
                if (!Launchers.Any())
                    return "No launcher";
                else if (Launchers.Count > 1)
                    return "Play...";
                else if (Launchers.First().Type == LaunchType.GxGamesLink)
                    return "Play (GX)";
                else if (Launchers.First().Type == LaunchType.WebLink)
                    return "Play (Web)";
                else
                    return "Play";
            }
        }

        public ICommand PlayCommand { get; }
        private void Play()
        {
            if (Launchers.Count == 0)
                return;
            else if (Launchers.Count == 1)
                EntryLauncher.Launch(Launchers.First());
            else
                PlaySelectionViewModel.ShowModal(Launchers);
        }

        // ------
        // Others
        // ------

        public JamReadmeViewModel? Readme { get; }
        public bool HasRequiredReadme => Readme != null && Readme.IsRequired;
        public bool HasRegularReadme => Readme != null && !Readme.IsRequired;
        public ICommand? OpenReadmeCommand => Readme?.OpenReadmeCommand;

        public JamAfterwordViewModel? Afterword { get; }
        public bool HasAfterword => Afterword != null;
        public ICommand? OpenAfterwordCommand => Afterword?.OpenAfterwordCommand;

        public JamThumbnailsViewModel? Thumbnails { get; }
        public ImageSource? Thumbnail => Thumbnails?.Thumbnail;
        public ImageSource? ThumbnailSmall => Thumbnails?.Thumbnail;

        // ---------
        // Directory
        // ---------
        
        public ICommand OpenDirectoryCommand { get; }
        private void OpenDirectory()
        {
            ProcessLauncher.OpenDirectory(Model.DirectoryPath);
        }
    }
}
