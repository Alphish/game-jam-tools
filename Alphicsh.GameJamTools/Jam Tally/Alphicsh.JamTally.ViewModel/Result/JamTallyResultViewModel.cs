using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Alphicsh.JamTally.Model.Result;
using Alphicsh.JamTally.Model.Result.Trophies;
using Alphicsh.JamTally.Model.Result.Trophies.Data;
using Alphicsh.JamTools.Common.Controls.Files;
using Alphicsh.JamTools.Common.IO;
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

            GenerateTallySheetsCommand = SimpleCommand.From(GenerateTallySheets);
            GenerateResultsPostCommand = SimpleCommand.From(GenerateResultsPost);

            GenerateTrophiesSpecificationCommand = SimpleCommand.From(TrophiesInput.Generate);
            GenerateTrophiesCoreTemplateCommand = SimpleCommand
                .From(() => PerformTrophiesImageOperation(Model.GenerateTrophiesCoreTemplate));
            GenerateTrophiesEntriesTemplateCommand = SimpleCommand
                .From(() => PerformTrophiesImageOperation(Model.GenerateTrophiesEntriesTemplate));
            CompileTrophiesCommand = SimpleCommand
                .From(() => PerformTrophiesImageOperation(Model.CompileTrophies));

            ExportTrophiesCommand = SimpleCommand.From(ExportTrophies);
        }

        public string FinalRankingText { get; }
        public string AwardRankingsText { get; }

        // ---------------
        // Text generators
        // ---------------

        public ICommand GenerateTrophiesSpecificationCommand { get; }

        public ICommand GenerateTallySheetsCommand { get; }
        private void GenerateTallySheets()
        {
            var directoryPath = FileQuery.OpenDirectory().GetPath();
            if (directoryPath == null)
                return;

            Model.GenerateTallySheets(directoryPath.Value);
        }

        public string ResultsPostText { get; private set; } = string.Empty;
        public ICommand GenerateResultsPostCommand { get; }
        private void GenerateResultsPost()
        {
            ResultsPostText = Model.GenerateResultsPost();
            RaisePropertyChanged(nameof(ResultsPostText));
        }

        // -------------------
        // Trophies generators
        // -------------------

        public ICommand GenerateTrophiesCoreTemplateCommand { get; }
        public ICommand GenerateTrophiesEntriesTemplateCommand { get; }
        public ICommand CompileTrophiesCommand { get; }

        private void PerformTrophiesImageOperation(Action<FilePath, FilePath> action)
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

            action(sourcePath.Value, destinationPath.Value);
        }

        public ICommand ExportTrophiesCommand { get; }
        private void ExportTrophies()
        {
            var sourcePath = FileQuery.OpenFile()
                .WithFileType("*.svg", "Scalable Vector Graphics")
                .GetPath();

            if (sourcePath == null)
                return;

            ExportProgressText = "";
            var trophiesExporter = new TrophiesExporter(sourcePath.Value);
            trophiesExporter.ExportProgress += OnExportProgress;
            Task.Run(trophiesExporter.Export);
        }

        public string ExportProgressText { get; private set; } = string.Empty;
        private void OnExportProgress(object? sender, TrophiesExportProgressEvent e)
        {
            var previousDetails = ExportProgressText != "" ? ExportProgressText.Substring(ExportProgressText.IndexOf("\n")) : "";
            ExportProgressText = $"Exported items: {e.ExportedItems}/{e.TotalItems}\n" + e.Message + previousDetails;
            RaisePropertyChanged(nameof(ExportProgressText));
        }
    }
}
