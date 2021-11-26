using System;

namespace Alphicsh.JamPlayer.Export.Parser.Scanning.Exceptions
{
    public class ExportScriptScanningException : Exception
    {
        public Cursor Location { get; }
        
        public ExportScriptScanningException(string message, Cursor location)
            : base($"Scanning error at {location.ToShortString()}: " + message)
        {
            Location = location;
        }
    }
}