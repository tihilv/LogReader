namespace LogReader.Search
{
    public struct SearchPosition
    {
        public readonly long Line;
        public readonly int Column;

        public readonly int? Start;
        public readonly int End;

        public SearchPosition(long line, int column, int? start, int end)
        {
            Line = line;
            Column = column;
            Start = start;
            End = end;
        }
    }
}
