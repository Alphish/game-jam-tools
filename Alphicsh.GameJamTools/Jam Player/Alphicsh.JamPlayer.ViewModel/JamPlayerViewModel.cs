using System;
using System.Windows.Input;
using Alphicsh.JamPlayer.Model;
using Alphicsh.JamPlayer.Model.Vote;
using Alphicsh.JamPlayer.ViewModel.Awards;
using Alphicsh.JamPlayer.ViewModel.Export;
using Alphicsh.JamPlayer.ViewModel.Jam;
using Alphicsh.JamPlayer.ViewModel.Ranking;
using Alphicsh.JamPlayer.ViewModel.Vote;
using Alphicsh.JamPlayer.ViewModel.Vote.Saving;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPlayer.ViewModel
{
    public class JamPlayerViewModel : AppViewModel<AppModel>
    {
        public static JamPlayerViewModel Current { get; private set; } = null!;

        public static JamPlayerViewModel Create(AppModel model)
        {
            if (Current != null)
                throw new InvalidOperationException("AppViewModel should be created only once.");

            Current = new JamPlayerViewModel(model);
            return Current;
        }

        private JamPlayerViewModel(AppModel model) : base(model)
        {
            VoteSaveSystem = new JamVoteSaveViewModel();

            JamProperty = MutableProperty.Create<JamOverviewViewModel>(this, nameof(Jam), default!);
            RankingProperty = MutableProperty.Create<RankingOverviewViewModel>(this, nameof(Ranking), default!);
            AwardsProperty = MutableProperty.Create<AwardsOverviewViewModel>(this, nameof(Awards), default!);
            ExporterProperty = MutableProperty.Create<ExporterViewModel>(this, nameof(Exporter), default!);

            RecreateViewModels();
            SaveRankingCommand = SimpleCommand.From(Model.PlayerDataManager.SaveRanking);
            SaveExporterCommand = SimpleCommand.From(Model.PlayerDataManager.SaveExporter);
        }

        private void RecreateViewModels()
        {
            Jam = new JamOverviewViewModel(Model.Jam);
            Ranking = new RankingOverviewViewModel(Model.Ranking);
            Awards = new AwardsOverviewViewModel(Model.Awards, Jam);
            Exporter = new ExporterViewModel(Model.Exporter);

            VoteSaveSystem.DropViewModel();
            if (Model.Jam.DirectoryPath != default)
            {
                Vote = new JamVoteViewModel(this, new JamVote());
                VoteSaveSystem.TrackViewModel(Vote);
                VoteSaveSystem.MarkUnmodified();
            }
        }

        public MutableProperty<JamOverviewViewModel> JamProperty { get; }
        public JamOverviewViewModel Jam { get => JamProperty.Value; private set => JamProperty.Value = value; }

        public MutableProperty<RankingOverviewViewModel> RankingProperty { get; }
        public RankingOverviewViewModel Ranking { get => RankingProperty.Value; private set => RankingProperty.Value = value; }

        public MutableProperty<AwardsOverviewViewModel> AwardsProperty { get; }
        public AwardsOverviewViewModel Awards { get => AwardsProperty.Value; private set => AwardsProperty.Value = value; }

        public MutableProperty<ExporterViewModel> ExporterProperty { get; }
        public ExporterViewModel Exporter { get => ExporterProperty.Value; private set => ExporterProperty.Value = value; }

        public JamVoteViewModel Vote { get; private set; } = default!;
        public JamVoteSaveViewModel VoteSaveSystem { get; }

        public ICommand SaveRankingCommand { get; }
        public ICommand SaveExporterCommand { get; }

        // --------------------
        // Available operations
        // --------------------

        public void LoadJamFromFile(FilePath filePath)
        {
            Model.LoadJamFromFile(filePath);
            RecreateViewModels();
        }

        public void ResetUserData()
        {
            Model.PlayerDataManager.ResetUserData();
            RecreateViewModels();
        }
    }
}
