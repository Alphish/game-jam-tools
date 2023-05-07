namespace Alphicsh.EntryPackager.Model.Entry
{
    public class JamEntryEditable
    {
        public JamEntryEditableData Data { get; set; } = new JamEntryEditableData();

        public string Title { get => Data.Title; set => Data.Title = value; }
        public JamTeamEditable Team { get => Data.Team; }
    }
}
