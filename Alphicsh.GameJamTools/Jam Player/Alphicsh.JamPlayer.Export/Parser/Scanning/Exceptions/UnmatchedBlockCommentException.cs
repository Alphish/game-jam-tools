namespace Alphicsh.JamPlayer.Export.Parser.Scanning.Exceptions
{
    public class UnmatchedBlockCommentException : ExportScriptScanningException
    {
        public UnmatchedBlockCommentException(Cursor location)
            : base("Unmatched block comment.", location) { }
    }
}