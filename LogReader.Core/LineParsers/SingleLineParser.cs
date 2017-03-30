namespace LogReader
{
    public class SingleLineParser: ILineParser
    {
        public byte ColumnCount => 1;
        public LogLine Parse(long index, string line)
        {
            return new LogLine(index, line, new string[] {line});
        }
    }
}
