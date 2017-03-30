using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LogReader.Gui;

namespace LogReader
{
    public partial class FileForm : Form
    {
        private LogContext _logContext;

        public FileForm()
        {
            InitializeComponent();

            
        }

        public void SetFileName(string fileName)
        {
            Text = fileName;

            _logContext = new LogContext(fileName);

            _logContext.LogProvider.LogChanged += LogProviderOnLogChanged;
            _logContext.LogProvider.LogAppended += LogProviderOnLogAppended;

            logListView.VirtualListSize = (int)_logContext.LogProvider.Count;

            EnsureLastItemVisible();
            logListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        public void SetParser(ILineParser parser)
        {
            _logContext.SetParser(parser);

            _listCache = null;
            logListView.Columns.Clear();
            logListView.Columns.Add("#", -2);
            for (int index = 0; index < parser.ColumnCount; index++)
                logListView.Columns.Add("Column" + index, -2);

            logListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        void EnsureLastItemVisible()
        {
            logListView.EnsureVisible(logListView.Items.Count - 1);
        }


        private void LogProviderOnLogAppended(object sender, LogChangedEventArgs logChangedEventArgs)
        {
            Delegate m = new Action(() =>
            {
                var visible = _listCache.Contains(logListView.Items[logListView.Items.Count - 1]);
                logListView.VirtualListSize = (int)_logContext.LogProvider.Count;
                if (visible)
                    EnsureLastItemVisible();

            });
            Invoke(m);
        }

        private void LogProviderOnLogChanged(object sender, LogChangedEventArgs logChangedEventArgs)
        {
            Delegate m = new Action(() =>
            {
                _listCache = null;
                logListView.VirtualListSize = (int)_logContext.LogProvider.Count;
                logListView.Invalidate();
            });
            Invoke(m);
        }

        private void logListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (_listCache != null && e.ItemIndex >= _firstCachedItem && e.ItemIndex < _firstCachedItem + _listCache.Length)
            {
                //A cache hit, so get the ListViewItem from the cache instead of making a new one.
                e.Item = _listCache[e.ItemIndex - _firstCachedItem];
            }
            else
            {
                e.Item = ProvideItem(e.ItemIndex);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _logContext.Dispose();
            base.OnClosed(e);
        }

        private ListViewItem[] _listCache;
        private int _firstCachedItem;
        private void logListView_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            if (_listCache != null && e.StartIndex >= _firstCachedItem && e.EndIndex <= _firstCachedItem + _listCache.Length)
                return;

            _firstCachedItem = e.StartIndex;
            int length = e.EndIndex - e.StartIndex + 1;
            _listCache = new ListViewItem[length];

            for (int i = 0; i < length; i++)
                _listCache[i] = ProvideItem(i + _firstCachedItem);
        }

        ListViewItem ProvideItem(long index)
        {
            string line = _logContext.LogProvider[index];
            LogLine logLine = _logContext.Parser.Parse(index, line);
            string[] lineStrings = new string[logLine.ParsedString.Length + 1];
            lineStrings[0] = index.ToString();
            Array.Copy(logLine.ParsedString, 0, lineStrings, 1, logLine.ParsedString.Length);
            ListViewItem result = new ListViewItem(lineStrings);
            LineFormat? format = _logContext.FormattingRuleManager.ApplyFormat(logLine);
            if (format != null)
            {
                result.ForeColor = format.Value.ForeColor ?? result.ForeColor;
                result.BackColor = format.Value.BackColor ?? result.BackColor;
                result.Font = format.Value.Font?? result.Font;
            }

            return result;
        }

        private void lineParsingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new LineParserOptionsDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                SetParser(LineParserFactory.GetLineParser());
        }

        private void formatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<FormattingRuleDefinition> definitions = new List<FormattingRuleDefinition>();
            foreach (FormattingRuleDefinition definition in _logContext.FormattingRuleManager.Definitions)
            {
                FormattingRuleDefinition copy = new FormattingRuleDefinition() {Action = definition.Action, Condition = definition.Condition, Enabled = definition.Enabled, Name = definition.Name, Priority = definition.Priority};
                definitions.Add(copy);
            }

            var dialog = new FormatEditingDialog();
            dialog.Rules = definitions;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _logContext.FormattingRuleManager.SetDefinitions(definitions);
                _listCache = null;
                logListView.Invalidate();
            }
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<FilteringRuleDefinition> definitions = new List<FilteringRuleDefinition>();
            foreach (FilteringRuleDefinition definition in _logContext.FilteringRuleManager.Definitions)
            {
                FilteringRuleDefinition copy = new FilteringRuleDefinition() { Condition = definition.Condition, Enabled = definition.Enabled, Name = definition.Name, Priority = definition.Priority };
                definitions.Add(copy);
            }

            var dialog = new FilterEditingDialog();
            dialog.Rules = definitions;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _logContext.FilteringRuleManager.SetDefinitions(definitions);
            }
        }

        private void toggleFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _logContext.FilteringRuleManager.Enabled = !_logContext.FilteringRuleManager.Enabled;
        }

        private void toggleFilterToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            toggleFilterToolStripMenuItem.Checked = _logContext.FilteringRuleManager.Enabled;
        }

        private void logListView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                CopySelectedValuesToClipboard();
        }

        private void CopySelectedValuesToClipboard()
        {
            var builder = new StringBuilder();
            foreach (int index in logListView.SelectedIndices)
                builder.AppendLine(_logContext.LogProvider[index]);

            Clipboard.SetText(builder.ToString());
        }

    }
}

