using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace LogReader
{
    public class FilteringRuleDefinition
    {
        private int _priority;
        private string _name;
        string _condition;
        private bool _enabled;

        IFilteringRule _compiledRule;

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

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public IFilteringRule GetCompiledRule(ILogEnvironment environment)
        {
            if (_compiledRule == null)
            {
                _compiledRule = ComplieRule();
                _compiledRule.SetEnvironment(environment);
            }
            return _compiledRule;
        }


        const string CodeTemplate = @"
    using System;
    using LogReader;

    namespace LogReader
    {
        public class {NAME}: IFilteringRule
        {
            ILogEnvironment e;
            void IFilteringRule.SetEnvironment(ILogEnvironment environment)
            {
                e = environment;
            }

            bool IFilteringRule.ShowLine(string l)
            {
                return {CONDITION};
            }
        }
    }
";


        public IFilteringRule ComplieRule()
        {
            var path = Directory.GetParent(Assembly.GetCallingAssembly().Location).FullName;

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();

            parameters.ReferencedAssemblies.Add(Path.Combine(path, "LogReader.Model.dll"));
            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = false;
            parameters.CompilerOptions = "/optimize";

            var className = "FilteringRule_" + GetHashCode();

            string code = CodeTemplate.Replace("{NAME}", className)
                                      .Replace("{CONDITION}", Condition);

            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);

            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();
                foreach (CompilerError error in results.Errors)
                    sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));

                throw new InvalidOperationException(sb.ToString());

            }

            Assembly assembly = results.CompiledAssembly;
            var type = assembly.GetType("LogReader."+ className);

            return (IFilteringRule)Activator.CreateInstance(type);
        }

        public FilteringRuleDefinition Clone()
        {
            var result = (FilteringRuleDefinition) MemberwiseClone();
            result._compiledRule = null;
            return result;
        }
    }
}
