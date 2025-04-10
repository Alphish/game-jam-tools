using System;
using System.Windows.Input;
using Alphicsh.JamTally.Model.Result;
using Alphicsh.JamTally.Trophies.Export;
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

            GenerateTrophiesCoreTemplateCommand = SimpleCommand
                .From(() => PerformTrophiesSaveOperation(Model.GenerateTrophiesCoreTemplate));
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

        public ICommand GenerateTallySheetsCommand { get; }
        private void GenerateTallySheets()
        {
            var exportPath = FileQuery.SaveFile()
                .WithFileType("*.xlsx", "Microsoft Excel Spreadsheet")
                .WithDefaultName("Results.xlsx")
                .GetPath();

            if (exportPath == null)
                return;

            Model.GenerateTallySheets(exportPath.Value);
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

        private void PerformTrophiesSaveOperation(Action<FilePath> action)
        {
            var jamDirectory = JamTallyViewModel.Current.Jam!.Model.DirectoryPath;
            var defaultFilename = $"{jamDirectory.GetLastSegmentName()} Trophies.core.svg";

            var destinationPath = FileQuery.SaveFile()
                .WithFileType("*.svg", "Scalable Vector Graphics")
                .WithDefaultName(defaultFilename)
                .GetPath();

            if (destinationPath == null)
                return;

            action(destinationPath.Value);
        }

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

        private static TallyTrophiesExporter TrophiesExporter { get; } = new TallyTrophiesExporter();

        public ICommand ExportTrophiesCommand { get; }
        private async void ExportTrophies()
        {
            var sourcePath = FileQuery.OpenFile()
                .WithFileType("*.svg", "Scalable Vector Graphics")
                .GetPath();

            if (sourcePath == null)
                return;

            ExportProgressText = "";

            var exportDirectory = sourcePath.Value.GetParentDirectoryPath().Append(sourcePath.Value.GetNameWithoutExtension());
            var progress = new Progress<TrophiesExportProgress>();
            progress.ProgressChanged += OnExportProgress;
            await TrophiesExporter.Export(sourcePath.Value, exportDirectory, progress);
        }

        public string ExportProgressText { get; private set; } = string.Empty;
        private void OnExportProgress(object? sender, TrophiesExportProgress e)
        {
            var previousDetails = ExportProgressText != "" ? ExportProgressText.Substring(ExportProgressText.IndexOf("\n")) : "";
            ExportProgressText = $"Exported items: {e.ExportedItems}/{e.TotalItems}\n" + e.Message + previousDetails;
            RaisePropertyChanged(nameof(ExportProgressText));
        }
    }
}
