using System;

namespace LogReader
{
    public struct LogLine : IEquatable<LogLine>
    {
        public readonly long Index;
        public readonly string SourceString;
        public readonly string[] ParsedString;

        public LogLine(long index, string sourceString, string[] parsedString)
        {
            Index = index;
            SourceString = sourceString;
            ParsedString = parsedString;
        }

        public bool Equals(LogLine other)
        {
            return Index == other.Index && string.Equals(SourceString, other.SourceString) && ParsedString.Length == other.ParsedString.Length;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is LogLine && Equals((LogLine) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Index.GetHashCode();
                hashCode = (hashCode * 397) ^ (SourceString != null ? SourceString.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ParsedString != null ? ParsedString.Length.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(LogLine left, LogLine right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LogLine left, LogLine right)
        {
            return !left.Equals(right);
        }
    }
}