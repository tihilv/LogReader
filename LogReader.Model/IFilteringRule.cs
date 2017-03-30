namespace LogReader
{
    public interface IFilteringRule
    {
        void SetEnvironment(ILogEnvironment environment);

        bool ShowLine(string line);
    }
}