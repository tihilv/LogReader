namespace LogReader
{
    public interface IFormattingRule
    {
        void SetEnvironment(ILogEnvironment environment);

        LineFormat? GetFormat(LogLine line);
    }
}