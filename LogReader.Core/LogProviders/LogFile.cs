using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;

namespace LogReader
{
    public class LogFile: ILogProvider
    {
        private readonly string _fileName;
        private readonly List<long> _lineStarts;

        private readonly FileSystemWatcher _watcher;

        private readonly StringReader _stream;

        private readonly object _lock = new object();

        private readonly Timer _timer;

        public LogFile(string fileName)
        {
            _fileName = fileName;
            _lineStarts = new List<long>();

            _stream = new StringReader(_fileName);

            FillLines();

            var fileInfo = new FileInfo(_fileName);
            _watcher = new FileSystemWatcher(fileInfo.Directory.FullName, fileInfo.Name);
            _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime;
            _watcher.Changed += (sender, args) =>
            {
                FillLines();
            };
            _watcher.EnableRaisingEvents = true;
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Interval = 1000;
            _timer.Elapsed += (sender, args) => FillLines();
            //_timer.Start();
        }

        public long Count
        {
            get
            {
                lock (_lock)
                {
                    return _lineStarts.Count;
                }

            }
        }


        void FillLines()
        {
            bool newLines;
            lock (_lock)
            {
                int lines = _lineStarts.Count;
                if (_lineStarts.Any())
                {
                    _stream.Seek(_lineStarts.Last());
                    _stream.ReadLine(true);
                }
                while (!_stream.EndOfStream)
                {
                    long start = _stream.Position;
                    _stream.ReadLine(true);
                    long end = _stream.Position;

                    _lineStarts.Add(start);
                }
                newLines = _lineStarts.Count != lines;
            }

            if (newLines)
                LogAppended?.Invoke(this, new LogChangedEventArgs());
        }

        public string this[long index]
        {
            get
            {
                _stream.Seek(_lineStarts[(int) index]);
                return _stream.ReadLine(false);
            }
        }

        public event EventHandler<LogChangedEventArgs> LogChanged;
        public event EventHandler<LogChangedEventArgs> LogAppended;

        public void Dispose()
        {
            _timer.Stop();
            _watcher.Dispose();
            _stream.Dispose();
        }
    }
}
