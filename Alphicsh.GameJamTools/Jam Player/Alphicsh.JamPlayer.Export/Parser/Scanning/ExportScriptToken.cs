namespace Alphicsh.JamPlayer.Export.Parser.Scanning
{
    public class ExportScriptToken
    {
        public ExportScriptTokenType Type { get; }
        public string Content { get; }
        public Cursor Start { get; }
        public Cursor End { get; }

        public ExportScriptToken(ExportScriptTokenType type, string content, Cursor start, Cursor end)
        {
            Type = type;
            Content = content;
            Start = start;
            End = end;
        }
    }
}