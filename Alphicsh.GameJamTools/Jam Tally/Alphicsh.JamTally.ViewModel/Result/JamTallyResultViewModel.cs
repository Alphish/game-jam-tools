using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Alphicsh.JamTally.Model.Result;
using Alphicsh.JamTally.Model.Result.Trophies;
using Alphicsh.JamTools.Common.Controls.Files;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.JamTally.ViewModel.Result
{
    public class JamTallyResultViewModel : WrapperViewModel<JamTallyResult>
    {
        public JamTallyResultViewModel(JamTallyResult model) : base(model)
        {
            FinalRankingText = model.GetFinalRankingText();
            AwardRankingsText = model.GetAwardRankingsText();

            GenerateRankingSheetCommand = SimpleCommand.From(GenerateRankingSheet);
            GenerateVotesSheetCommand = SimpleCommand.From(GenerateVotesSheet);
            GenerateTrophiesTemplateCommand = SimpleCommand.From(GenerateTrophiesTemplate);
            ExportTrophiesCommand = SimpleCommand.From(ExportTrophies);
            GenerateResultsPostCommand = SimpleCommand.From(GenerateResultsPost);
        }

        public string FinalRankingText { get; }
        public string AwardRankingsText { get; }

        // ----------
        // Generators
        // ----------

        public ICommand GenerateRankingSheetCommand { get; }
        private void GenerateRankingSheet()
        {
            var rankingSheet = Model.GenerateRankingSheet();
            var filePath = FileQuery.SaveFile()
                .WithFileType("*.csv", "Comma-separated values")
                .WithDefaultName("Ranking.csv")
                .GetPath();

            if (filePath == null)
                return;

            File.WriteAllText(filePath.Value.Value, rankingSheet);
        }

        public ICommand GenerateVotesSheetCommand { get; }
        private void GenerateVotesSheet()
        {
            var votesSheet = Model.GenerateVotesSheet();
            var filePath = FileQuery.SaveFile()
                .WithFileType("*.csv", "Comma-separated values")
                .WithDefaultName("Votes.csv")
                .GetPath();

            if (filePath == null)
                return;

            File.WriteAllText(filePath.Value.Value, votesSheet);
        }

        public ICommand GenerateTrophiesTemplateCommand { get; }
        private void GenerateTrophiesTemplate()
        {
            var sourcePath = FileQuery.OpenFile()
                .WithFileType("*.svg", "Scalable Vector Graphics")
                .GetPath();

            if (sourcePath == null)
                return;

            var destinationPath = FileQuery.SaveFile()
                .WithFileType("*.svg", "Scalable Vector Graphics")
                .WithDefaultName(sourcePath.Value.GetNameWithoutExtension() + ".new.svg")
                .GetPath();

            if (destinationPath == null)
                return;

            Model.GenerateTrophiesTemplate(sourcePath.Value, destinationPath.Value);
        }

        public ICommand ExportTrophiesCommand { get; }
        private void ExportTrophies()
        {
            var sourcePath = FileQuery.OpenFile()
                .WithFileType("*.svg", "Scalable Vector Graphics")
                .GetPath();

            if (sourcePath == null)
                return;

            ResultsPostText = "";
            var trophiesExporter = new TrophiesExporter(sourcePath.Value);
            trophiesExporter.ExportProgress += OnExportProgress;
            Task.Run(trophiesExporter.Export);
        }

        private void OnExportProgress(object? sender, TrophiesExportProgressEvent e)
        {
            var previousDetails = ResultsPostText != "" ? ResultsPostText.Substring(ResultsPostText.IndexOf("\n")) : "";
            ResultsPostText = $"Exported items: {e.ExportedItems}/{e.TotalItems}\n" + e.Message + previousDetails;
            RaisePropertyChanged(nameof(ResultsPostText));
        }

        public string ResultsPostText { get; private set; }
        public ICommand GenerateResultsPostCommand { get; }
        private void GenerateResultsPost()
        {
            ResultsPostText = Model.GenerateResultsPost();
            RaisePropertyChanged(nameof(ResultsPostText));
        }
    }
}
