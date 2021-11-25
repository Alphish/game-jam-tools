using System;

namespace Alphicsh.JamPlayer.Export.Parser.Parsing.Exceptions
{
    public class ExportScriptParsingException : Exception
    {
        public Cursor Start { get; }
        public Cursor End { get; }
        
        public ExportScriptParsingException(string message, Cursor start, Cursor end)
            : base($"Parsing error at {start.ToShortString()}: " + message)
        {
            Start = start;
            End = end;
        }
    }
}