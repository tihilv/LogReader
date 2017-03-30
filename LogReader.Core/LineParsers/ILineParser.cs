namespace LogReader
{
    public interface ILineParser
    {
        byte ColumnCount { get; }

        LogLine Parse(long index, string line);
    }
}