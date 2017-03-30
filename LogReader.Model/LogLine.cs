namespace LogReader
{
    public struct LogLine
    {
        public readonly long Index;
        public readonly string SourceString;
        public readonly string[] ParsedString;

        public LogLine(long index, string sourceString, string[] parsedString)
        {
            Index = index;
            SourceString = sourceString;
            ParsedString = parsedString;
        }
    }
}