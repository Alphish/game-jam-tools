namespace Alphicsh.JamTally.Model.Result.Trophies
{
    public class TrophiesExportProgressEvent
    {
        public int ExportedItems { get; init; }
        public int TotalItems { get; init; }
        public string Message { get; init; } = default!;
    }
}
