using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LogReader.Gui
{
    public partial class FormatEditingDialog : Form
    {
        private List<FormattingRuleDefinition> _rules;

        public FormatEditingDialog()
        {
            InitializeComponent();
        }

        internal List<FormattingRuleDefinition> Rules
        {
            get
            {
                return _rules;
            }
            set
            {
                _rules = value;
                formattingRuleDefinitionBindingSource.DataSource = value;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            formattingRuleDefinitionBindingSource.AddNew();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            formattingRuleDefinitionBindingSource.RemoveAt(rulesListBox.SelectedIndex);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < _rules.Count; index++)
            {
                FormattingRuleDefinition ruleDefinition = _rules[index];
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
