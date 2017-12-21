using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Fluent;
using LogReader;
using LogReader.Common;
using LogReader.Search;
using LogReader.Serialize;
using Microsoft.Win32;

namespace LogReaderWPF
{
    public partial class MainWindow
    {
        private readonly OptionsTracker _optionsTracker;

        private readonly ObservableCollection<LogTabData> _tabs;
        internal static readonly object _lock = new object();

        public MainWindow()
        {
            _tabs = new ObservableCollection<LogTabData>();
            DataContext = _tabs;
            _optionsTracker = new OptionsTracker(Assembly.GetExecutingAssembly().Location + ".options.xml");

            InitializeComponent();
            

            BindingOperations.EnableCollectionSynchronization(_tabs, _lock);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Rect bounds = LogReader.Properties.Settings.Default.MainWindowPosition;
            Top = bounds.Top;
            Left = bounds.Left;

            if (SizeToContent == SizeToContent.Manual)
            {
                Width = bounds.Width;
                Height = bounds.Height;
            }


            if (!String.IsNullOrEmpty(App.OpenFileName) && File.Exists(App.OpenFileName))
                OpenFile(App.OpenFileName);
        }

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Log Files (*.log)|*.log|All Files (*.*)|*.*";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog(this)??false)
            {
                foreach (string fileName in openFileDialog.FileNames)
                    OpenFile(fileName);
            }
        }

        internal void OpenFile(string fileName)
        {
            LogTabData tab = new LogTabData(fileName, _optionsTracker.Options);
            _tabs.Add(tab);
            logTabControl.SelectedIndex = logTabControl.Items.Count - 1;
        }

        void CloseFile()
        {
            LogTabData tab = _tabs[logTabControl.SelectedIndex];
            CloseFile(tab);
        }

        void CloseFile(LogTabData tab)
        {
            _tabs.Remove(tab);
            tab.Dispose();
        }


        private void SetFilter()
        {
            if (logTabControl.SelectedIndex < 0)
                return;

            ObservableCollection<FilteringRuleDefinition> definitions = new ObservableCollection<FilteringRuleDefinition>();
            foreach (FilteringRuleDefinition definition in _optionsTracker.Options.FilteringRuleDefinitions)
                definitions.Add(definition.Clone());

            FilterWindow filterWindow = new FilterWindow();
            filterWindow.Owner = this;
            filterWindow.SetCollection(definitions);
            if (filterWindow.ShowDialog()??false)
                _optionsTracker.Options.FilteringRuleDefinitions = new List<FilteringRuleDefinition>(definitions);
        }

        private void ToggleFilter()
        {
            foreach (LogTabData tab in _tabs)
                tab.Context.FilteringRuleManager.Enabled = toggleFilterButton.IsChecked ?? false;
        }

        private void SetFormat()
        {
            ObservableCollection<FormattingRuleDefinition> definitions = new ObservableCollection<FormattingRuleDefinition>();
            foreach (FormattingRuleDefinition definition in _optionsTracker.Options.FormattingRuleDefinitions)
                definitions.Add(definition.Clone());

            FormatWindow filterWindow = new FormatWindow();
            filterWindow.Owner = this;
            filterWindow.SetCollection(definitions);
            if (filterWindow.ShowDialog() ?? false)
                _optionsTracker.Options.FormattingRuleDefinitions = new List<FormattingRuleDefinition>(definitions);
        }

        private bool _isInSearch;
        private void SearchClick(Func<Searcher, SearchFilter, SearchPosition?, SearchPosition?> method)
        {
            try
            {
                _isInSearch = true;
                LogTabData tab = _tabs[logTabControl.SelectedIndex];
                LogContext context = tab.Context;
                StringComparison isCaseSensitive = (caseSensitive.IsChecked == true)
                    ? StringComparison.InvariantCulture
                    : StringComparison.InvariantCultureIgnoreCase;
                SearchFilter filter = new SearchFilter(searchBox.Text, isCaseSensitive);
                SearchPosition? nextPosition = method(context.Searcher, filter, tab.LastPosition);
                if (nextPosition != null)
                {
                    tab.LastPosition = nextPosition;
                    ScrollToIndex(tab, (int) nextPosition.Value.Line);
                }
            }
            finally
            {
                _isInSearch = false;
            }

        }

        private void SelectPosition(long line)
        {
            if (!_isInSearch)
            {
                LogTabData tab = _tabs[logTabControl.SelectedIndex];
                tab.LastPosition = new SearchPosition(line, 0, null, 0);
            }
        }

        void ScrollToIndex(LogTabData tab, int index)
        {
            var item = VisualSearch.FindVisualChild<ListView>(logTabControl, view => view.DataContext == tab);
            item.SelectedIndex = index;

            var vsp = VisualSearch.FindVisualChild<ScrollViewer>(item);
            double scrollHeight = vsp.ScrollableHeight;

            double offset = scrollHeight * index / item.Items.Count+vsp.ViewportHeight/2;

            vsp.ScrollToVerticalOffset(offset);

        }

        private void SetFilterForSearch()
        {
            string culture = (caseSensitive.IsChecked == true)
                ? "StringComparison.InvariantCulture"
                : "StringComparison.InvariantCultureIgnoreCase";

            var condition = "l.IndexOf(\"" + searchBox.Text.Replace("\"", "\\\"") + "\", " + culture + ") >= 0";
            var filterRule = new FilteringRuleDefinition() {Condition = condition, Enabled = true, Name = searchBox.Text, Priority = 0};
            var collection = _optionsTracker.Options.FilteringRuleDefinitions;
            collection.Add(filterRule);
            _optionsTracker.Options.FilteringRuleDefinitions = new List<FilteringRuleDefinition>(collection);

            if (toggleFilterButton.IsChecked != true)
            {
                toggleFilterButton.IsChecked = true;
                ToggleFilter();
            }
        }
        private void SetParser()
        {
            LogTabData tab = _tabs[logTabControl.SelectedIndex];

            LineParserOptionsWindow filterWindow = new LineParserOptionsWindow();
            filterWindow.Owner = this;
            filterWindow.DataContext = tab.FileOptions.ParserOptions.Clone();
            if (filterWindow.ShowDialog() ?? false)
            {
                tab.Context.SetParser(filterWindow.Options.CreateParser());
                tab.FileOptions.ParserOptions = filterWindow.Options;
                var index = logTabControl.SelectedIndex;
                DataContext = null;
                DataContext = _tabs;
                logTabControl.SelectedIndex = index;
                if (filterWindow.Default)
                    _optionsTracker.Options.DefaultParserOptions = filterWindow.Options;
            }
        }

        private void CommandOpen_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFile();
        }

        private void CommandClose_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var logTabData = (e.OriginalSource as FrameworkElement)?.DataContext as LogTabData;
            if (logTabData == null)
                CloseFile();
            else
                CloseFile(logTabData);
        }

        private void Search_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            searchBox.Focus();
        }

        private void SearchNext_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SearchClick((searcher, filter, lastPosition) => searcher.GetNext(filter, lastPosition));
        }

        private void SearchPrev_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SearchClick((searcher, filter, lastPosition) => searcher.GetPrev(filter, lastPosition));
        }

        private void ToggleFilter_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(e.OriginalSource is ToggleButton))
                toggleFilterButton.IsChecked = !toggleFilterButton.IsChecked;
            ToggleFilter();
        }

        private void SetFilter_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SetFilter();
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                CustomCommands.SearchNext.Execute(null, null);
        }
        
        private void logListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                ViewItem item = (ViewItem)e.AddedItems[0];

                SelectPosition(item.Id);
            }
        }

        private void SetFilterForSearch_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SetFilterForSearch();
        }

        private void SetFormat_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SetFormat();
        }

        private void SetParser_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SetParser();
        }

        private void RibbonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LogReader.Properties.Settings.Default.MainWindowPosition = RestoreBounds;
            LogReader.Properties.Settings.Default.Save();
        }

        private void CloseAll_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            for (int i = _tabs.Count-1; i >= 0; i--)
                CloseFile(_tabs[i]);
        }

        private void Exit_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateColumnsWidth(sender as ListView);
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateColumnsWidth(sender as ListView);
        }

        private void UpdateColumnsWidth(ListView listView)
        {
            var columns = ((GridView) listView.View).Columns;
            int autoFillColumnIndex = columns.Count - 1;
            
            if (Double.IsNaN(listView.ActualWidth))
                listView.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));

            double remainingSpace = listView.ActualWidth - 2 * SystemParameters.VerticalScrollBarWidth;
            for (int i = 0; i < columns.Count; i++)
                if (i != autoFillColumnIndex)
                    remainingSpace -= columns[i].ActualWidth;
            columns[autoFillColumnIndex].Width = remainingSpace >= 0 ? remainingSpace : 0;
        }
    }
}
