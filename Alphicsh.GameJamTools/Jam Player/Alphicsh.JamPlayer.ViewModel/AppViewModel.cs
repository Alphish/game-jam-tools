using System.Windows.Input;

using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;

using Alphicsh.JamPlayer.Model;
using Alphicsh.JamPlayer.ViewModel.Jam;
using Alphicsh.JamPlayer.ViewModel.Ranking;
using Alphicsh.JamPlayer.ViewModel.Awards;
using Alphicsh.JamPlayer.ViewModel.Export;

namespace Alphicsh.JamPlayer.ViewModel
{
    public class AppViewModel : WrapperViewModel<AppModel>
    {
        public AppViewModel(AppModel model)
            : base(model)
        {
            RecreateViewModels();
            SaveRankingCommand = new SimpleCommand(Model.PlayerDataManager.SaveRanking);
        }

        private void RecreateViewModels()
        {
            Jam = new JamOverviewViewModel(Model.Jam);
            Ranking = new RankingOverviewViewModel(Model.Ranking);
            Awards = new AwardsOverviewViewModel(Model.Awards, Jam);
            Exporter = new ExporterViewModel(Model.Exporter);
        }

        public JamOverviewViewModel Jam { get; private set; } = default!;
        public RankingOverviewViewModel Ranking { get; private set; } = default!;
        public AwardsOverviewViewModel Awards { get; private set; } = default!;
        public ExporterViewModel Exporter { get; private set; } = default!;
        public ICommand SaveRankingCommand { get; }

        // --------------------
        // Available operations
        // --------------------

        public void LoadJamFromFile(FilePath filePath)
        {
            Model.LoadJamFromFile(filePath);
            RecreateViewModels();
        }
    }
}
