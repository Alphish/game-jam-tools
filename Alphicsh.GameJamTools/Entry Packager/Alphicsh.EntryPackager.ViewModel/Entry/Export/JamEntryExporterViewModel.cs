using Alphicsh.EntryPackager.Model.Entry.Export;
using Alphicsh.JamTools.Common.Controls.Files;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;
using Alphicsh.JamTools.Common.ViewModel.Checklists;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Export
{
    public class JamEntryExporterViewModel : WrapperViewModel<JamEntryExporter>
    {
        public JamEntryExporterViewModel(JamEntryExporter model) : base(model)
        {
            Checklist = new ChecklistViewModel(model.Checklist);

            ExportCommand = ConditionalCommand.From(CanExport, Export);
            ExportCommand.ExecutionDependingOn(Checklist.IsReadyProperty);
        }

        public ChecklistViewModel Checklist { get; }

        public IConditionalCommand ExportCommand { get; }
        private bool CanExport() => Model.CanExport();
        private void Export()
        {
            var defaultName = Model.GetDefaultPackageName();
            var zipPath = FileQuery.SaveFile()
                .FromDirectory(Model.EntryData.Files.DirectoryPath.GetParentDirectoryPath())
                .WithFileType("*.zip", "ZIP archive")
                .WithDefaultName(defaultName)
                .GetPath();

            if (zipPath == null)
                return;

            Model.ExportTo(zipPath.Value);
        }

        public void RefreshChecklist()
        {
            Checklist.Recheck();
            ExportCommand.RaiseCanExecuteChanged();
        }
    }
}
