namespace LogReader.FormattingRules
{
    class DummyRule: IFormattingRule
    {
        public static readonly DummyRule Instance = new DummyRule();

        private DummyRule()
        {
        }

        public void SetEnvironment(ILogEnvironment environment)
        {
            
        }

        public LineFormat? GetFormat(LogLine line)
        {
            return null;
        }
    }
}
