using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            FillDebug();
        }

        public ReadOnlyCollection<FormattingRuleDefinition> Definitions
        {
            get { return _definitions.AsReadOnly(); }
        }

        public void SetDefinitions(IEnumerable<FormattingRuleDefinition> definitions)
        {
            _definitions.Clear();
            _definitions.AddRange(definitions);

            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void FillDebug()
        {
            FormattingRuleDefinition t = new FormattingRuleDefinition();
            t.Name = "WarningRule";
            t.Condition = "l.Contains(\"Trace\")";
            t.Action = "BackColor = Color.Red, ForeColor = Color.White, Font = new Font(\"Courier New\", 12)";
            t.Enabled = true;

            _definitions.Add(t);
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
