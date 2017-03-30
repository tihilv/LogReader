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
        private readonly List<LogLinePosition> _lines;

        private readonly FileSystemWatcher _watcher;

        private readonly StringReader _stream;

        private readonly object _lock = new object();

        private readonly Timer _timer;

        public LogFile(string fileName)
        {
            _fileName = fileName;
            _lines = new List<LogLinePosition>();

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
            _timer.Start();
        }

        public long Count
        {
            get
            {
                lock (_lock)
                {
                    return _lines.Count;
                }

            }
        }


        void FillLines()
        {
            var lines = _lines.Count;

            lock (_lock)
            {
                if (_lines.Any())
                {
                    _stream.Seek(_lines.Last().Start);
                    _stream.ReadLine();
                }
                while (!_stream.EndOfStream)
                {
                    long start = _stream.Position;
                    _stream.ReadLine();
                    long end = _stream.Position;

                    _lines.Add(new LogLinePosition(start, end));
                }
            }

            if (_lines.Count != lines)
                    LogAppended?.Invoke(this, new LogChangedEventArgs());
        }

        public string this[long index]
        {
            get
            {
                _stream.Seek(_lines[(int) index].Start);
                return _stream.ReadLine();
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

    public struct LogLinePosition
    {
        public readonly long Start;
        public readonly long End;

        public LogLinePosition(long start, long end)
        {
            Start = start;
            End = end;
        }
    }
}
