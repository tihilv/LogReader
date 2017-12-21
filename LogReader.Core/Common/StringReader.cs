using System;
using System.IO;
using System.Text;

namespace LogReader
{
    class StringReader: IDisposable
    {
        const int BufferLength = 102400;
        private long _read;
        private long _index;
        private readonly byte[] _buffer = new byte[BufferLength];

        readonly Stream _underlinedStream;

        public StringReader(string fileName)
        {
            _underlinedStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }

        public bool EndOfStream => Position >= _underlinedStream.Length || (_underlinedStream.Position == _underlinedStream.Length && _read == 0);
        public long Position { get; private set; }

        public void Seek(long position)
        {
            _underlinedStream.Seek(position, SeekOrigin.Begin);
            Position = position;
            _read = 0;
        }

        public string ReadLine(bool onlySeek)
        {
            bool found = false;

            int additionalSymbols = 0;
            StringBuilder sb = onlySeek ? null : new StringBuilder();
            int readSymbolCount = 0;

            while (!found)
            {
                if (_read <= 0)
                {
                    // Read next block
                    _index = 0;
                    _read = _underlinedStream.Read(_buffer, 0, BufferLength);
                    if (_read == 0)
                    {
                        if (readSymbolCount > 0) 
                            break;
                        
                        return null;
                    }
                }

                for (long max = _index + _read; _index < max;)
                {
                    char ch = (char)_buffer[_index];
                    _read--; _index++;

                    if (ch == '\0' || ch == '\n')
                    {
                        additionalSymbols ++;
                        found = true;
                        break;
                    }
                    else if (ch == '\r')
                    {
                        additionalSymbols++;
                        continue;
                    }
                    else
                    {
                        sb?.Append(ch);
                        readSymbolCount++;
                    }
                }
            }

            var result = sb?.ToString();
            Position += readSymbolCount+additionalSymbols;
            Position = Math.Min(Position, _underlinedStream.Length);
            return result;
        }

        public void Dispose()
        {
            _underlinedStream.Dispose();
        }
    }
}
