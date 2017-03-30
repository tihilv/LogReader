using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Threading;
using LogReaderWPF;

namespace LogReader.Common
{
    public class LogItemsList: IList<ViewItem>, IList, INotifyCollectionChanged
    {
        private readonly SynchronizationContext _uiContext;

        private readonly LogContext _logContext;

        public LogItemsList(LogContext logContext)
        {
            _logContext = logContext;
            _logContext.LogProvider.LogAppended += LogProviderOnLogAppended;
            _logContext.LogProvider.LogChanged += LogProviderOnLogChanged;

            _uiContext = SynchronizationContext.Current;
        }

        private void LogProviderOnLogAppended(object sender, LogChangedEventArgs logChangedEventArgs)
        {
            _uiContext.Send(state =>
            {
                lock (MainWindow._lock)
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }, null);
        }

        private void LogProviderOnLogChanged(object sender, LogChangedEventArgs logChangedEventArgs)
        {
            _uiContext.Send(state =>
            {
                lock (MainWindow._lock)
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }, null);
        }

        #region IList<ViewItem>

        public IEnumerator<ViewItem> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return ((IList<ViewItem>)this)[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ViewItem item)
        {
            throw new NotImplementedException();
        }

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            return  false;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            return -1;
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(ViewItem item)
        {
            return false;
        }

        public void CopyTo(ViewItem[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ViewItem item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            object[] o = (object[]) array;
            IList<ViewItem> t = this;
            for (int i = 0; i < Count; i++)
            {
                o[i + index] = t[i];
            }
        }

        public int Count
        {
            get
            {
                var result = (int) _logContext.LogProvider.Count;
                return result;
            }
        }

        public object SyncRoot => null;
        public bool IsSynchronized => false;
        public bool IsReadOnly => true;
        public bool IsFixedSize => false;

        public int IndexOf(ViewItem item)
        {
            return -1;
        }

        public void Insert(int index, ViewItem item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        ViewItem IList<ViewItem>.this[int index]
        {
            get
            {
                var line = _logContext.LogProvider[index]??String.Empty;
                LogLine logLine = _logContext.Parser.Parse(index, line);
                var format = _logContext.FormattingRuleManager.ApplyFormat(logLine);
                ViewItem result = new ViewItem(index, logLine, format?.ForeColor?.Name??"Black", format?.BackColor?.Name ?? "White", format?.Font);
                return result;
            }
            set { throw new NotImplementedException(); }
        }

        object IList.this[int index]
        {
            get { return ((IList<ViewItem>) this)[index]; }
            set { throw new NotImplementedException(); }
        }

#endregion

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }

    public struct ViewItem
    {
        public long Id { get; private set; }
        public LogLine Value { get; private set; }
        public string Foreground { get; private set; }
        public string Background { get; private set; }
        public string FontFamily { get; private set; }
        public float FontSize { get; private set; }

        public ViewItem(long id, LogLine value, string foreground, string background, Font font)
        {
            Id = id;
            Value = value;
            Foreground = foreground;
            Background = background;
                FontFamily = (font?.FontFamily??(System.Drawing.FontFamily.GenericMonospace)).Name;
                FontSize = font?.Size??12;
        }

        public string this[int index]
        {
            get { return Value.ParsedString[index]; }
        }
    }
}
