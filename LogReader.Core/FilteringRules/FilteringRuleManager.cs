using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LogReader
{
    public class FilteringRuleManager
    {
        readonly List<FilteringRuleDefinition> _definitions;
        private bool _enabled;

        private ILogEnvironment _logEnvironment;

        public event EventHandler<EventArgs> Changed;

        public FilteringRuleManager()
        {
            _definitions = new List<FilteringRuleDefinition>();
        }

        public ReadOnlyCollection<FilteringRuleDefinition> Definitions
        {
            get { return _definitions.AsReadOnly(); }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                Changed?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SetDefinitions(IEnumerable<FilteringRuleDefinition> definitions)
        {
            _definitions.Clear();
            _definitions.AddRange(definitions);

            Changed?.Invoke(this, EventArgs.Empty);
        }

        public bool FilterLine(string line)
        {
            if (!Enabled)
                return true;

            var sorted = _definitions.Where(d=>d.Enabled).OrderBy(d=>d.Priority);
            return !sorted.Any() || sorted.Any(d => d.GetCompiledRule(_logEnvironment).ShowLine(line));
        }
    }
}
