namespace Alphicsh.JamTally.Model.Jam.Loading
{
    public class JamEntryOverride
    {
        public string EntryId { get; init; } = default!;
        public string? TallyCode { get; set; }
        public string? TallyTitle { get; set; }
        public string? TallyAuthors { get; set; }
    }
}
