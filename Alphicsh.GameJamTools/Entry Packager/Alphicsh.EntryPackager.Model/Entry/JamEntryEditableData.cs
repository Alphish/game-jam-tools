namespace Alphicsh.EntryPackager.Model.Entry
{
    public class JamEntryEditableData
    {
        public string Title { get; set; } = default!;
        public JamTeamEditable Team { get; } = new JamTeamEditable();
    }
}
