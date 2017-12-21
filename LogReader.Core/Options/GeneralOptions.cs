using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using LogReader.Annotations;

namespace LogReader.Options
{
    public class GeneralOptions: INotifyPropertyChanged
    {
        private LineParserOptions _defaultParserOptions;

        private List<FileOptions> _fileOptions;

        private List<FormattingRuleDefinition> _formattingRuleDefinitions;
        private List<FilteringRuleDefinition> _filteringRuleDefinitions;

        public GeneralOptions()
        {
            _defaultParserOptions = new LineParserOptions();
            _fileOptions = new List<FileOptions>();

            _formattingRuleDefinitions = new List<FormattingRuleDefinition>();
            _filteringRuleDefinitions = new List<FilteringRuleDefinition>();
        }

        internal void InitOptions()
        {
            foreach (FileOptions options in _fileOptions)
                options.PropertyChanged += OptionsOnPropertyChanged;
        }

        public LineParserOptions DefaultParserOptions
        {
            get { return _defaultParserOptions; }
            set
            {
                if (_defaultParserOptions != value)
                {
                    _defaultParserOptions = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<FileOptions> FileOptions
        {
            get { return _fileOptions; }
            set
            {
                if (_fileOptions != value)
                {
                    _fileOptions = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<FormattingRuleDefinition> FormattingRuleDefinitions
        {
            get { return _formattingRuleDefinitions; }
            set
            {
                if (_formattingRuleDefinitions != value)
                {
                    _formattingRuleDefinitions = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<FilteringRuleDefinition> FilteringRuleDefinitions
        {
            get { return _filteringRuleDefinitions; }
            set
            {
                if (_filteringRuleDefinitions != value)
                {
                    _filteringRuleDefinitions = value;
                    OnPropertyChanged();
                }
            }
        }

        public FileOptions GetFileOptions(string fileName)
        {
            FileOptions options = _fileOptions.FirstOrDefault(f => f.FileName.Equals(fileName, StringComparison.InvariantCultureIgnoreCase));
            if (options == null)
            {
                options = new FileOptions()
                {
                    FileName = fileName,
                    ParserOptions = _defaultParserOptions
                };
                options.PropertyChanged += OptionsOnPropertyChanged;
                _fileOptions.Add(options);
                OnPropertyChanged(nameof(FileOptions));
            }

            return options;
        }

        private void OptionsOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            OnPropertyChanged(nameof(FileOptions));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
