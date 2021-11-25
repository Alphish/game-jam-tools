namespace Alphicsh.JamPlayer.Export.Parser
{
    public readonly struct Cursor
    {
        public int Position { get; } // 0-based
        public int Line { get; } // 0-based
        public int Column { get; } // 0-based

        public Cursor(int position, int line, int column)
        {
            Position = position;
            Line = line;
            Column = column;
        }

        public Cursor AdvanceColumns(int count)
            => new Cursor(Position + count, Line, Column + count);

        public Cursor AdvanceLine()
            => new Cursor(Position + 1, Line + 1, 0);

        public override string ToString()
        {
            return $"Line {Line+1}, Column {Column+1}, Position {Position+1}";
        }

        public string ToShortString()
        {
            return $"Line {Line + 1}, Column {Column + 1}";
        }
    }
}