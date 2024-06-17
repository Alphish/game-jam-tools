using System.Windows.Input;
using Alphicsh.JamTally.Model;
using Alphicsh.JamTally.ViewModel.Jam;
using Alphicsh.JamTally.ViewModel.Vote;
using Alphicsh.JamTools.Common.Controls.Files;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.JamTally.ViewModel
{
    public class JamTallyViewModel : AppViewModel<JamTallyModel>
    {
        public static JamTallyViewModel Current => (JamTallyViewModel)AppViewModel.Current;

        public JamTallyViewModel(JamTallyModel model) : base(model)
        {
            OpenJamDirectoryCommand = SimpleCommand.From(OpenJamDirectory);

            VoteManager = new JamVoteManagerViewModel(model.VoteManager);
        }

        public bool HasJam => Model.HasJam;
        public bool HasAlignments => Model.HasAlignments;

        public JamOverviewViewModel? Jam { get; private set; }
        public JamVoteCollectionViewModel? VotesCollection { get; private set; }

        public JamVoteManagerViewModel VoteManager { get; }

        // -------
        // Loading
        // -------

        public ICommand OpenJamDirectoryCommand { get; }
        private void OpenJamDirectory()
        {
            var directoryPath = FileQuery.OpenDirectory().GetPath();
            if (directoryPath == null)
                return;

            LoadJamDirectory(directoryPath.Value);
        }

        public void LoadJamDirectory(FilePath directoryPath)
        {
            Model.LoadDirectory(directoryPath);
            UpdateJamViewModel();
        }

        private void UpdateJamViewModel()
        {
            Jam = new JamOverviewViewModel(Model.Jam!);
            VotesCollection = new JamVoteCollectionViewModel(Model.VotesCollection!);
            RaisePropertyChanged(nameof(HasJam), nameof(HasAlignments), nameof(Jam), nameof(VotesCollection));
        }
    }
}
