using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogReader.Gui
{
    public partial class FilterEditingDialog : Form
    {
        private List<FilteringRuleDefinition> _rules;

        public FilterEditingDialog()
        {
            InitializeComponent();
        }

        internal List<FilteringRuleDefinition> Rules
        {
            get
            {
                return _rules;
            }
            set
            {
                _rules = value;
                FiltertingRuleDefinitionBindingSource.DataSource = value;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            FiltertingRuleDefinitionBindingSource.AddNew();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            FiltertingRuleDefinitionBindingSource.RemoveAt(rulesListBox.SelectedIndex);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < _rules.Count; index++)
            {
                FilteringRuleDefinition ruleDefinition = _rules[index];
                try
                {
                    ruleDefinition.ComplieRule();
                }
                catch (Exception ex)
                {
                    rulesListBox.SelectedIndex = index;
                    DialogResult = DialogResult.None;
                    MessageBox.Show("Cannot compile:\n" + ex.Message);
                }
            }
        }
    }
}
