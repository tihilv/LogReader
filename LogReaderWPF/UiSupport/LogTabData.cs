using System;
using System.Collections.Generic;
using LogReader;
using LogReader.Common;
using LogReader.Options;
using LogReader.Search;

namespace LogReaderWPF
{
    class LogTabData : IDisposable
    {
        private GeneralOptions _options;

        internal LogTabData(string fileName, GeneralOptions options)
        {
            Header = fileName;
            _options = options;
            FileOptions = options.GetFileOptions(fileName);

            Context = new LogContext(fileName, FileOptions.ParserOptions.CreateParser());
            InitContext();
            Data = new LogItemsList(Context);
        }

        private void InitContext()
        {
            _options.PropertyChanged += OnOptionsPropertyChanged;
            Context.FormattingRuleManager.SetDefinitions(_options.FormattingRuleDefinitions);
            Context.FilteringRuleManager.SetDefinitions(_options.FilteringRuleDefinitions);
        }

        private void OnOptionsPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_options.FormattingRuleDefinitions))
                Context.FormattingRuleManager.SetDefinitions(_options.FormattingRuleDefinitions);

            else if (e.PropertyName == nameof(_options.FilteringRuleDefinitions))
                Context.FilteringRuleManager.SetDefinitions(_options.FilteringRuleDefinitions);
        }

        public string Header { get; private set; }
        public LogContext Context { get; private set; }
        public IList<ViewItem> Data { get; private set; }

        public SearchPosition? LastPosition { get; set; }

        public FileOptions FileOptions { get; set; }

        public void Dispose()
        {
            _options.PropertyChanged -= OnOptionsPropertyChanged;
            Context.Dispose();
        }
    }
}