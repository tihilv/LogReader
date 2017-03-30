using System;
using System.Windows.Forms;

namespace LogReader
{
    public partial class LineParserOptionsDialog : Form
    {
        public LineParserOptionsDialog()
        {
            InitializeComponent();

            SetControlsEnabled();
        }

        private void csvParser_CheckedChanged(object sender, EventArgs e)
        {
            SetControlsEnabled();
        }

        private void SetControlsEnabled()
        {
            separatorChar.Enabled = csvParser.Checked;
            columnCount.Enabled = csvParser.Checked;
        }
        
        private void okButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
        }
    }

    static class LineParserFactory
    {
        public static ILineParser GetLineParser()
        {
            if (Properties.Settings.Default.LineParser_Type_Name == 0)
                return new SingleLineParser();
            else if (Properties.Settings.Default.LineParser_Type_Name == 1)
            {
                return new CsvParser(Properties.Settings.Default.LineParser_CSV_Separator[0], (byte)Properties.Settings.Default.LineParser_CSV_Columns);
            }

            return null;
        }
    }
}
