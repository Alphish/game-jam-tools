namespace Alphicsh.JamPlayer.Export.Parser.Scanning.Exceptions
{
    public class UnmatchedStringException : ExportScriptScanningException
    {
        public UnmatchedStringException(Cursor location)
            : base($"A string must be matched by its starting delimiter in the same line.", location) { }
    }
}