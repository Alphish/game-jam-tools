using Alphicsh.EntryPackager.Model.Entry.Export;
using Alphicsh.JamTools.Common.Mvvm;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Export
{
    public class JamEntryExporterViewModel : WrapperViewModel<JamEntryExporter>
    {
        public JamEntryExporterViewModel(JamEntryExporter model) : base(model)
        {
            Checklist = new JamEntryChecklistViewModel(model.Checklist);
        }

        public JamEntryChecklistViewModel Checklist { get; }
        }
    }
}
