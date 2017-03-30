using System;

namespace LogReader.Search
{
    public class Searcher
    {
        private readonly LogContext _logContext;

        public Searcher(LogContext logContext)
        {
            _logContext = logContext;
        }

        public SearchPosition? GetNext(SearchFilter searchFilter, SearchPosition? prev = null)
        {
            long startLine = Math.Min(prev?.Line ?? 0, _logContext.LogProvider.Count - 1);
            int startColumn = prev?.Column ?? 0;
            int startIndex = (prev?.Start??(-1)) + 1;

            string searchString = searchFilter.String ?? String.Empty;

            for (long l = startLine; l < _logContext.LogProvider.Count; l++)
            {
                var parsedLine = _logContext.Parser.Parse(l, _logContext.LogProvider[l]);
                for (int c = startColumn;  c< parsedLine.ParsedString.Length; c++)
                {
                    string t = parsedLine.ParsedString[c];
                    var result = t.IndexOf(searchString, Math.Min(startIndex, t.Length), searchFilter.StringComparision);
                    if (result >=0)
                        return new SearchPosition(l, c, result, result + searchString.Length);

                    startIndex = 0;
                }
                startColumn = 0;
            }

            return null;
        }

        public SearchPosition? GetPrev(SearchFilter searchFilter, SearchPosition? prev = null)
        {
            long startLine = Math.Min(prev?.Line ??long.MaxValue, _logContext.LogProvider.Count-1);
            int startColumn = prev?.Column ?? _logContext.Parser.ColumnCount-1;
            int? startIndex = (prev?.Start ?? 1) -1;

            string searchString = searchFilter.String ?? String.Empty;

            for (long l = startLine; l >=0; l--)
            {
                var parsedLine = _logContext.Parser.Parse(l, _logContext.LogProvider[l]);
                for (int c = startColumn; c >=0; c--)
                {
                    string t = parsedLine.ParsedString[c];
                    var result = t.LastIndexOf(searchString, Math.Min(startIndex?? t.Length-1, t.Length), searchFilter.StringComparision);
                    if (result >= 0)
                        return new SearchPosition(l, c, result, result + searchString.Length);

                    startIndex = null;
                }
                startColumn = _logContext.Parser.ColumnCount - 1;
            }

            return null;
        }
    }
}
