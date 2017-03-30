using System.Windows;
using LogReaderWPF;

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
    }
}
