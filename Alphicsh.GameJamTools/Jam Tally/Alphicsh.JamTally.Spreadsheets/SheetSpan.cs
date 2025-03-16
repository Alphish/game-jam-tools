namespace Alphicsh.JamTally.Spreadsheets
{
    public struct SheetSpan
    {
        public int From { get; }
        public int To { get; }
        public int Length { get; }

        public SheetSpan(int from, int length)
        {
            From = from;
            To = from + length - 1;
            Length = length;
        }

        public SheetSpan(SheetSpan before, int length) : this(before.From + before.Length, length)
        {
        }

        public SheetSpan(SheetSpan before, int offset, int length) : this(before.From + before.Length + offset, length)
        {
        }
    }
}
