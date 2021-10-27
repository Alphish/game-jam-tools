using Alphicsh.JamTools.Common.Mvvm;

using Alphicsh.JamPlayer.Model;
using Alphicsh.JamPlayer.Model.Jam;
using Alphicsh.JamPlayer.ViewModel.Jam;
using Alphicsh.JamPlayer.ViewModel.Ranking;

namespace Alphicsh.JamPlayer.ViewModel
{
    public class AppViewModel : BaseViewModel<AppModel>
    {
        public AppViewModel(AppModel model)
            : base(model)
        {
            RecreateViewModels();
        }

        private void RecreateViewModels()
        {
            Jam = new JamOverviewViewModel(Model.Jam);
            Ranking = new RankingOverviewViewModel(Model.Ranking);
        }

        public JamOverviewViewModel Jam { get; private set; } = default!;
        public RankingOverviewViewModel Ranking { get; private set; } = default!;

        // --------------------
        // Available operations
        // --------------------

        // TODO: Load Jam from a data file, rather than a model instance
        public void LoadJam(JamOverview jam)
        {
            Model.LoadJam(jam);
            RecreateViewModels();
        }
    }
}
