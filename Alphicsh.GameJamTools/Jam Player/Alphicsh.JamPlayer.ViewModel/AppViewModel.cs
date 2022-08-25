using System;
using System.Windows.Input;
using Alphicsh.JamPlayer.Model;
using Alphicsh.JamPlayer.ViewModel.Awards;
using Alphicsh.JamPlayer.ViewModel.Export;
using Alphicsh.JamPlayer.ViewModel.Jam;
using Alphicsh.JamPlayer.ViewModel.Ranking;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPlayer.ViewModel
{
    public class AppViewModel : WrapperViewModel<AppModel>
    {
        public static AppViewModel Current { get; private set; } = null!;

        public static AppViewModel Create(AppModel model)
        {
            if (Current != null)
                throw new InvalidOperationException("AppViewModel should be created only once.");

            Current = new AppViewModel(model);
            return Current;
        }

        private AppViewModel(AppModel model) : base(model)
        {
            JamProperty = MutableProperty.Create<JamOverviewViewModel>(this, nameof(Jam), default!);
            RankingProperty = MutableProperty.Create<RankingOverviewViewModel>(this, nameof(Ranking), default!);
            AwardsProperty = MutableProperty.Create<AwardsOverviewViewModel>(this, nameof(Awards), default!);
            ExporterProperty = MutableProperty.Create<ExporterViewModel>(this, nameof(Exporter), default!);

            RecreateViewModels();
            SaveRankingCommand = new SimpleCommand(Model.PlayerDataManager.SaveRanking);
            SaveExporterCommand = new SimpleCommand(Model.PlayerDataManager.SaveExporter);
            HasOverlayProperty = MutableProperty.Create(this, nameof(HasOverlay), false);
        }

        private void RecreateViewModels()
        {
            Jam = new JamOverviewViewModel(Model.Jam);
            Ranking = new RankingOverviewViewModel(Model.Ranking);
            Awards = new AwardsOverviewViewModel(Model.Awards, Jam);
            Exporter = new ExporterViewModel(Model.Exporter);
        }

        public MutableProperty<JamOverviewViewModel> JamProperty { get; }
        public JamOverviewViewModel Jam { get => JamProperty.Value; private set => JamProperty.Value = value; }

        public MutableProperty<RankingOverviewViewModel> RankingProperty { get; }
        public RankingOverviewViewModel Ranking { get => RankingProperty.Value; private set => RankingProperty.Value = value; }

        public MutableProperty<AwardsOverviewViewModel> AwardsProperty { get; }
        public AwardsOverviewViewModel Awards { get => AwardsProperty.Value; private set => AwardsProperty.Value = value; }

        public MutableProperty<ExporterViewModel> ExporterProperty { get; }
        public ExporterViewModel Exporter { get => ExporterProperty.Value; private set => ExporterProperty.Value = value; }

        public ICommand SaveRankingCommand { get; }
        public ICommand SaveExporterCommand { get; }

        public MutableProperty<bool> HasOverlayProperty { get; }
        public bool HasOverlay { get => HasOverlayProperty.Value; set => HasOverlayProperty.Value = value; }

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
