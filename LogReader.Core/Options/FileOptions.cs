using System.ComponentModel;
using System.Runtime.CompilerServices;
using LogReader.Annotations;

namespace LogReader.Options
{
    public class FileOptions : INotifyPropertyChanged
    {
        private string _fileName;

        LineParserOptions _parserOptions;

        public FileOptions()
        {
            _parserOptions = new LineParserOptions();
        }

        public LineParserOptions ParserOptions
        {
            get { return _parserOptions; }
            set
            {
                if (_parserOptions != value)
                {
                    _parserOptions = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}