using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;
using LogReader.FormattingRules;
using Microsoft.CSharp;

namespace LogReader
{
    public class FormattingRuleDefinition
    {
        private int _priority;
        private string _name;
        private string _condition;
        private string _action;
        private bool _enabled;

        IFormattingRule _compiledRule;

        public int Priority
        {
            get { return _priority; }
            set
            {
                if (_priority != value)
                {
                    _compiledRule = null;
                    _priority = value;
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _compiledRule = null;
                    _name = value;
                }
            }
        }


        public string Condition
        {
            get { return _condition; }
            set
            {
                if (_condition != value)
                {
                    _compiledRule = null;
                    _condition = value;
                }
            }
        }

        public string Action
        {
            get { return _action; }
            set
            {
                if (_action != value)
                {
                    _compiledRule = null;
                    _action = value;
                }
            }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public IFormattingRule GetCompiledRule(ILogEnvironment environment)
        {
            if (_compiledRule == null)
            {
                try
                {
                    _compiledRule = ComplieRule();
                    _compiledRule.SetEnvironment(environment);
                }
                catch
                {
                    _compiledRule = DummyRule.Instance;
                }
            }
            return _compiledRule;
        }


        const string CodeTemplate = @"
    using System;
    using LogReader;
    using System.Drawing;

    namespace LogReader
    {
        public class {NAME}: IFormattingRule
        {
            ILogEnvironment e;
            void IFormattingRule.SetEnvironment(ILogEnvironment environment)
            {
                e = environment;
            }

            LineFormat? IFormattingRule.GetFormat(LogLine line)
            {
                string l = line.SourceString;
                string[] c = line.ParsedString;
                if ({CONDITION})
                    return new LineFormat(){{ACTION}};

                return null;
            }
        }
    }
";


        public IFormattingRule ComplieRule()
        {
            var path = Directory.GetParent(Assembly.GetCallingAssembly().Location).FullName;
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();

            parameters.ReferencedAssemblies.Add(Path.Combine(path, "LogReader.Model.dll"));
            parameters.ReferencedAssemblies.Add("System.Drawing.dll");
            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = false;
            parameters.CompilerOptions = "/optimize";

            var className = "FormattingRule_" + GetHashCode();

            string code = CodeTemplate.Replace("{NAME}", className)
                .Replace("{CONDITION}", Condition)
                .Replace("{ACTION}", Action);

            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);

            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();
                foreach (CompilerError error in results.Errors)
                    sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));

                throw new InvalidOperationException(sb.ToString());

            }

            Assembly assembly = results.CompiledAssembly;
            var type = assembly.GetType("LogReader." + className);

            return (IFormattingRule) Activator.CreateInstance(type);
        }

        public FormattingRuleDefinition Clone()
        {
            var result = (FormattingRuleDefinition)MemberwiseClone();
            result._compiledRule = null;
            return result;
        }
    }
}
