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
            _lastCount = (int)_logContext.LogProvider.Count;
        }

        private int _lastCount = 0;

        private void LogProviderOnLogAppended(object sender, LogChangedEventArgs logChangedEventArgs)
        {
            _uiContext.Send(state =>
            {
                lock (MainWindow._lock)
                { 
                    _lastCount = (int)_logContext.LogProvider.Count;
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }, null);
        }

        private void LogProviderOnLogChanged(object sender, LogChangedEventArgs logChangedEventArgs)
        {
            _uiContext.Send(state =>
            {
                lock (MainWindow._lock)
                {
                    _lastCount = (int)_logContext.LogProvider.Count;
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
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
            return (int)((ViewItem)value).Id;
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

        public int Count => _lastCount;

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

    public struct ViewItem : IEquatable<ViewItem>
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
            FontFamily = (font?.FontFamily ?? (System.Drawing.FontFamily.GenericMonospace)).Name;
            FontSize = font?.Size ?? 12;
        }

        public string this[int index]
        {
            get { return Value.ParsedString[index]; }
        }

        public bool Equals(ViewItem other)
        {
            return Id == other.Id && Value.Equals(other.Value) && string.Equals(Foreground, other.Foreground) && string.Equals(Background, other.Background) && string.Equals(FontFamily, other.FontFamily) && FontSize.Equals(other.FontSize);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ViewItem && Equals((ViewItem) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ Value.GetHashCode();
                hashCode = (hashCode * 397) ^ (Foreground != null ? Foreground.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Background != null ? Background.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FontFamily != null ? FontFamily.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ FontSize.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(ViewItem left, ViewItem right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ViewItem left, ViewItem right)
        {
            return !left.Equals(right);
        }
    }
}
