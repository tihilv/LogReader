using System;
using System.Linq;

namespace LogReader
{
    public class CsvParser : ILineParser
    {
        readonly char _separator;
        private readonly byte _columnCount;

        public CsvParser(char separator, byte columnCount)
        {
            _separator = separator;
            _columnCount = columnCount;
        }

        public byte ColumnCount => _columnCount;

        public LogLine Parse(long index, string line)
        {
            var splitted = line?.Split(_separator)??new string[0];

            if (splitted.Length > _columnCount)
            {
                splitted[_columnCount - 1] = String.Join(_separator.ToString(), splitted.Skip(_columnCount - 1));
            }
            if (splitted.Length < _columnCount)
            {
                string[] result = new string[_columnCount];
                int startIndex = result.Length - splitted.Length;
                for (int i = 0; i < startIndex; i++)
                    result[i] = String.Empty;

                Array.Copy(splitted, 0, result, startIndex, splitted.Length);
                splitted = result;
            }

            return new LogLine(index, line, splitted);
        }
    }
}
