using System;
using LogReader.Search;

namespace LogReader
{
    public class LogContext: IDisposable
    {
        private ILineParser _parser;
        private readonly ILogProvider _logProvider;

        private readonly FormattingRuleManager _formattingRuleManager;
        private readonly FilteringRuleManager _filteringRuleManager;

        private readonly Searcher _searcher;

        public LogContext(string fileName)
        {
            _formattingRuleManager = new FormattingRuleManager();
            _filteringRuleManager = new FilteringRuleManager();
            _searcher = new Searcher(this);

            _parser = new CsvParser('|', 4);
            _logProvider = new FilterLogProvider(new LogFileCache(new LogFile(fileName)), _filteringRuleManager);
        }

        public ILineParser Parser => _parser;

        public ILogProvider LogProvider => _logProvider;

        public FormattingRuleManager FormattingRuleManager => _formattingRuleManager;

        public FilteringRuleManager FilteringRuleManager => _filteringRuleManager;

        public Searcher Searcher => _searcher;

        public void SetParser(ILineParser parser)
        {
            _parser = parser;
        }

        public void Dispose()
        {
            _logProvider.Dispose();
        }
    }
}
