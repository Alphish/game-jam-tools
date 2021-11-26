namespace Alphicsh.JamPlayer.Export.Parser.Scanning.Exceptions
{
    public class UnknownEscapeSequenceException : ExportScriptScanningException
    {
        public string Sequence { get; }

        public UnknownEscapeSequenceException(string sequence, Cursor location)
            : base($"Unknown escape sequence '{sequence}'.", location)
        {
            Sequence = sequence;
        }
    }
}