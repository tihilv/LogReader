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

        public ILineParser Parser
        {
            get { return _parser; }
        }

        public ILogProvider LogProvider
        {
            get { return _logProvider; }
        }

        public FormattingRuleManager FormattingRuleManager
        {
            get { return _formattingRuleManager; }
        }

        public FilteringRuleManager FilteringRuleManager
        {
            get { return _filteringRuleManager; }
        }

        public void SetParser(ILineParser parser)
        {
            _parser = parser;
        }

        public Searcher Searcher
        {
            get { return _searcher; }
        }

        public void Dispose()
        {
            _logProvider.Dispose();
        }
    }
}
