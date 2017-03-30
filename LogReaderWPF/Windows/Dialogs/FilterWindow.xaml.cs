using System.Collections.ObjectModel;
using System.Windows;

namespace LogReader
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        private ObservableCollection<FilteringRuleDefinition> _rules;

        private readonly UiSerializeHelper<FilteringRuleDefinition> _serializeHelper;

        public FilterWindow()
        {
            InitializeComponent();
            _serializeHelper = new UiSerializeHelper<FilteringRuleDefinition>("Filter Options|*.filter.xml");
        }

        public void SetCollection(ObservableCollection<FilteringRuleDefinition> rules)
        {
            _rules = rules;
            listBox.ItemsSource = _rules;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            _rules.Add(new FilteringRuleDefinition() { Name = "Rule_" + _rules.Count });
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            _rules.Remove((FilteringRuleDefinition)listBox.SelectedItem);
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
