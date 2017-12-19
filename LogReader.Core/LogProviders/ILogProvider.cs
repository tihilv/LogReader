using System;

namespace LogReader
{
    public interface ILogProvider: IDisposable
    {
        long Count { get; }

        string this[long index] { get; }

        event EventHandler<LogChangedEventArgs> LogAppended;
        event EventHandler<LogChangedEventArgs> LogChanged;
    }

    public class LogChangedEventArgs : EventArgs
    {
        public new static readonly LogChangedEventArgs Empty = new LogChangedEventArgs();
    }
}

