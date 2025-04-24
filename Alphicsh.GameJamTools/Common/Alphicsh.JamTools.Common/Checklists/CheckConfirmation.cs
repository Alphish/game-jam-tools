namespace Alphicsh.JamTools.Common.Checklists
{
    public class CheckConfirmation
    {
        public bool IsConfirmed { get; set; } = false;
        public bool IsMandatory { get; init; } = true;
        public string Description { get; init; } = default!;
    }
}
