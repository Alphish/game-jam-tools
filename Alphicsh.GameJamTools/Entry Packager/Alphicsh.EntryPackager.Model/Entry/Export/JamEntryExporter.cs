namespace Alphicsh.EntryPackager.Model.Entry.Export
{
    public class JamEntryExporter
    {
        public JamEntryEditable EntryData { get; }
        public JamEntryChecklist Checklist { get; }

        public JamEntryExporter(JamEntryEditable entryData)
        {
            EntryData = entryData;
            Checklist = new JamEntryChecklist(entryData);
        }
    }
}
