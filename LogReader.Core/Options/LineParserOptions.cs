namespace LogReader.Options
{
    public class LineParserOptions
    {
        public bool Single { get; set; }
        public string Separator { get; set; }
        public byte Columns { get; set; }

        public LineParserOptions()
        {
            Single = true;
            Separator = "|";
            Columns = 1;
        }

        public ILineParser CreateParser()
        {
            if (Single)
                return new SingleLineParser();

            return new CsvParser(Separator, Columns);
        }

        public LineParserOptions Clone()
        {
            return (LineParserOptions)MemberwiseClone();
        }
    }
}
