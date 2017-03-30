using System;

namespace LogReader
{
    public class LogFileCache: ILogProvider
    {
        readonly ILogProvider _internal;
        private readonly RingCache<String> _cache;
        private readonly object _lock = new object();

        public LogFileCache(ILogProvider @internal, int cachedItems = 100000)
        {
            _internal = @internal;
            _internal.LogChanged += InternalOnLogChanged;
            _internal.LogAppended += InternalOnLogAppended;
            _cache = new RingCache<string>(cachedItems, i => _internal[i]);
        }

        private void InternalOnLogChanged(object sender, LogChangedEventArgs logChangedEventArgs)
        {
            lock (_lock)
            {
                _cache.Clear();
            }
            LogChanged?.Invoke(this, logChangedEventArgs);
        }

        private void InternalOnLogAppended(object sender, LogChangedEventArgs logChangedEventArgs)
        {
            LogAppended?.Invoke(this, logChangedEventArgs);
        }

        public void Dispose()
        {
            _internal.Dispose();
        }

        public long Count
        {
            get
            {
                lock (_lock)
                {
                    return _internal.Count;
                }
            }
        }

        public string this[long index]
        {
            get
            {
                lock (_lock)
                {
                    return _cache[index];
                }
            }
        }

        public event EventHandler<LogChangedEventArgs> LogChanged;
        public event EventHandler<LogChangedEventArgs> LogAppended;
    }
}
