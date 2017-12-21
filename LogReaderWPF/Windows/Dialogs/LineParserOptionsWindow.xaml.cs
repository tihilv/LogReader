using System.Windows;
using LogReader.Options;

namespace LogReader
{
    /// <summary>
    /// Interaction logic for LineParserOptionsWindow.xaml
    /// </summary>
    public partial class LineParserOptionsWindow : Window
    {
        public LineParserOptionsWindow()
        {
            InitializeComponent();
        }

        public LineParserOptions Options
        {
            get { return (LineParserOptions) DataContext; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public bool Default { get; set; }
    }
}
