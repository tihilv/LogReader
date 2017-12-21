using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using LogReader.Options;

namespace LogReader.Serialize
{
    public class OptionsTracker: IDisposable
    {
        private readonly string _fileName;

        private readonly BaseRuleSerializer<GeneralOptions> _serializer;
        readonly GeneralOptions _options;

        public OptionsTracker(string fileName)
        {
            _fileName = fileName;
            _serializer = new BaseRuleSerializer<GeneralOptions>();

            if (File.Exists(_fileName))
            {
                try
                {
                    using (var reader = new StreamReader(_fileName))
                        _options = _serializer.Deserialize(reader.ReadToEnd()).FirstOrDefault();
                }
                catch { }
            }
            if (_options == null)
                _options = new GeneralOptions();

            _options.InitOptions();
            _options.PropertyChanged += OptionsOnPropertyChanged;
        }

        public void Dispose()
        {
            _options.PropertyChanged -= OptionsOnPropertyChanged;
        }

        private void OptionsOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            using (var writer = new StreamWriter(_fileName))
            {
                var result = _serializer.Serialize(new[] {_options});
                writer.Write(result);
            }
        }

        public GeneralOptions Options
        {
            get { return _options; }
        }
    }
}
