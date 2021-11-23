using System;

namespace Alphicsh.JamPlayer.Export.Runtime.Expressions
{
    public class ExpressionCompilationException : Exception
    {
        public ExpressionCompilationException(string message)
            : base(message) { }
    }
}