using System;
using System.Collections.Generic;
using System.Linq;

namespace LogReader
{
    public class FormattingRuleManager
    {
        readonly List<FormattingRuleDefinition> _definitions;

        private ILogEnvironment _logEnvironment;

        public event EventHandler<EventArgs> Changed;

        public FormattingRuleManager()
        {
            _definitions = new List<FormattingRuleDefinition>();
        }

        public void SetDefinitions(IEnumerable<FormattingRuleDefinition> definitions)
        {
            _definitions.Clear();
            _definitions.AddRange(definitions);

            Changed?.Invoke(this, EventArgs.Empty);
        }

        public LineFormat? ApplyFormat(LogLine line)
        {
            var sorted = _definitions.Where(d=>d.Enabled).OrderBy(d=>d.Priority);
            foreach (FormattingRuleDefinition definition in sorted)
            {
                var rule = definition.GetCompiledRule(_logEnvironment);
                LineFormat? result = rule.GetFormat(line);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
