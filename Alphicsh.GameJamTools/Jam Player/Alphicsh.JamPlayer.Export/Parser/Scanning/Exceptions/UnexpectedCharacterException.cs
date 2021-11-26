namespace Alphicsh.JamPlayer.Export.Parser.Scanning.Exceptions
{
    public class UnexpectedCharacterException : ExportScriptScanningException
    {
        public char Character { get; }

        public UnexpectedCharacterException(char character, Cursor location)
            : base($"Unexpected character '{character}'.", location)
        {
            Character = character;
        }
    }
}