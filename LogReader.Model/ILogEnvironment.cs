using System.Dynamic;

namespace LogReader
{
    public interface ILogEnvironment
    {
        LogLine GetLogLine(long index);
    }
}
