﻿using System;
using System.Collections.Generic;
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

        public bool CanFilterBeApplied()
        {
            return Enabled && _definitions.Any(d => d.Enabled);
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
