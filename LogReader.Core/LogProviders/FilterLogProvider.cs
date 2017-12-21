using System;
using System.Collections.Generic;

namespace LogReader
{
    public class FilterLogProvider: ILogProvider
    {
        private readonly ILogProvider _internal;
        private readonly FilteringRuleManager _filteringRuleManager;

        private long _lastCheckedIndex;
        private readonly List<long> _mapping;

        private readonly object _lock = new object();

        public FilterLogProvider(ILogProvider @internal, FilteringRuleManager filteringRuleManager)
        {
            _internal = @internal;
            _internal.LogChanged += InternalOnLogChanged;
            _internal.LogAppended += InternalOnLogAppended;
            _mapping = new List<long>();
            _filteringRuleManager = filteringRuleManager;
            _filteringRuleManager.Changed += FilteringRuleManagerOnChanged;
            Filter();
        }

        private void FilteringRuleManagerOnChanged(object sender, EventArgs eventArgs)
        {
            OnLogChanged(LogChangedEventArgs.Empty);
        }

        private void InternalOnLogAppended(object sender, LogChangedEventArgs logChangedEventArgs)
        {
            lock (_lock)
            {
                Filter();
            }
            LogAppended?.Invoke(this, logChangedEventArgs);
        }

        private void InternalOnLogChanged(object sender, LogChangedEventArgs logChangedEventArgs)
        {
            OnLogChanged(logChangedEventArgs);
        }

        private void OnLogChanged(LogChangedEventArgs logChangedEventArgs)
        {
            lock (_lock)
            {
                _lastCheckedIndex = 0;
                _mapping.Clear();
                Filter();
            }
            LogChanged?.Invoke(this, logChangedEventArgs);
        }

        void Filter()
        {
            for (long i = _lastCheckedIndex; i < _internal.Count; i++)
                if (!_filteringRuleManager.CanFilterBeApplied() || _filteringRuleManager.FilterLine(_internal[i]))
                    _mapping.Add(i);

            _lastCheckedIndex = _internal.Count;
        }

        public void Dispose()
        {
            _filteringRuleManager.Changed -= FilteringRuleManagerOnChanged;
            _internal.LogAppended -= InternalOnLogAppended;
            _internal.LogChanged -= InternalOnLogChanged;

            _internal.Dispose();
        }

        public long Count
        {
            get
            {
                lock (_lock)
                    return _mapping.Count;
            }
        }

        

        public string this[long index] => _internal[_mapping[(int)index]];

        public event EventHandler<LogChangedEventArgs> LogAppended;
        public event EventHandler<LogChangedEventArgs> LogChanged;
    }
}
