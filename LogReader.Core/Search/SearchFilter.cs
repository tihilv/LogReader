using System;

namespace LogReader.Search
{
    public struct SearchFilter
    {
        public readonly string String;
        public readonly StringComparison StringComparision;

        public SearchFilter(string s, StringComparison stringComparision)
        {
            String = s;
            StringComparision = stringComparision;
        }
    }
}
