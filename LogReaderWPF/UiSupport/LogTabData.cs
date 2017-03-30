using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using LogReader;
using LogReader.Common;
using LogReader.Search;

namespace LogReaderWPF
{
    class LogTabData : IDisposable
    {
        internal LogTabData(string fileName)
        {
            Header = fileName;
            Context = new LogContext(fileName);
            InitContext();
            Data = new LogItemsList(Context);
            LineParserOptions = new LineParserOptions();
        }

        readonly string _formattingRuleFileName = Assembly.GetExecutingAssembly().Location + ".format.xml";
        readonly string _filteringRuleFileName = Assembly.GetExecutingAssembly().Location + ".filter.xml";

        private void InitContext()
        {
            try
            {
                ObservableCollection<FormattingRuleDefinition> formattingCollection =
                    new ObservableCollection<FormattingRuleDefinition>();
                UiSerializeHelper<FormattingRuleDefinition>.LoadFromFile(formattingCollection, _formattingRuleFileName);
                Context.FormattingRuleManager.SetDefinitions(formattingCollection);
            }
            catch (Exception)
            {

            }

            try
            {
                ObservableCollection<FilteringRuleDefinition> filteringCollection = new ObservableCollection<FilteringRuleDefinition>();
                UiSerializeHelper<FilteringRuleDefinition>.LoadFromFile(filteringCollection, _filteringRuleFileName);
                Context.FilteringRuleManager.SetDefinitions(filteringCollection);
            }
            catch (Exception)
            {
            }
        }

        private void SaveContext()
        {
            UiSerializeHelper<FormattingRuleDefinition>.SaveToFile(Context.FormattingRuleManager.Definitions, _formattingRuleFileName);

            UiSerializeHelper<FilteringRuleDefinition>.SaveToFile(Context.FilteringRuleManager.Definitions, _filteringRuleFileName);
        }

        public string Header { get; private set; }
        public LogContext Context { get; private set; }
        public IList<ViewItem> Data { get; private set; }

        public SearchPosition? LastPosition { get; set; }

        public LineParserOptions LineParserOptions { get; set; }

        public void Dispose()
        {
            SaveContext();
            Context.Dispose();
        }
    }

    public class LineParserOptions
    {
        public bool Single { get; set; }
        public string Separator { get; set; }
        public byte Columns { get; set; }

        public LineParserOptions()
        {
            Single = false;
            Separator = "|";
            Columns = 4;
        }

        public ILineParser CreateParser()
        {
            if (Single)
                return new SingleLineParser();

            return new CsvParser(Separator[0], Columns);
        }

        public LineParserOptions Clone()
        {
            return (LineParserOptions) MemberwiseClone();
        }
    }
}