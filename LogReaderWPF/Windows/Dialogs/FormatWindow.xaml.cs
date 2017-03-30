using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace LogReader
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FormatWindow : Window
    {
        private ObservableCollection<FormattingRuleDefinition> _rules;

        private readonly UiSerializeHelper<FormattingRuleDefinition> _serializeHelper;

        public FormatWindow()
        {
            InitializeComponent();
            _serializeHelper = new UiSerializeHelper<FormattingRuleDefinition>("Format Options|*.format.xml");
        }

        public void SetCollection(ObservableCollection<FormattingRuleDefinition> rules)
        {
            _rules = rules;
            listBox.ItemsSource = _rules;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            _rules.Add(new FormattingRuleDefinition() { Name = "Rule_" + _rules.Count });
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            _rules.Remove((FormattingRuleDefinition)listBox.SelectedItem);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true; //Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _serializeHelper.Save(_rules, this);
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            _serializeHelper.Load(_rules, this);
        }
    }
}
