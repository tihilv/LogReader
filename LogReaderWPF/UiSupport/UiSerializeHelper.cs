using System.Collections.Generic;

namespace LogReader
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text;
    using System.Windows;

    using Microsoft.Win32;

    class UiSerializeHelper<T>
    {
        private readonly string _filter;

        public UiSerializeHelper(string filter)
        {
            this._filter = filter;
        }

        public void Save(ObservableCollection<T> items, Window window)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = this._filter;
            if (dialog.ShowDialog(window) == true)
            {
                SaveToFile(items, dialog.FileName);
            }
        }

        public static void SaveToFile(IEnumerable<T> items, string fileName)
        {
            BaseRuleSerializer<T> serializer = new BaseRuleSerializer<T>();
            var serializedFormat = serializer.Serialize(items);
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                writer.Write(serializedFormat);
            }
        }

        public void Load(ObservableCollection<T> items, Window window)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = this._filter;
            if (dialog.ShowDialog(window) == true)
            {
                LoadFromFile(items, dialog.FileName);
            }
        }

        public static void LoadFromFile(ObservableCollection<T> items, string fileName)
        {
            BaseRuleSerializer<T> serializer = new BaseRuleSerializer<T>();
            using (StreamReader reader = new StreamReader(fileName, Encoding.UTF8))
            {
                var serializedFormat = reader.ReadToEnd();
                var rules = serializer.Deserialize(serializedFormat);
                items.Clear();
                foreach (T rule in rules)
                    items.Add(rule);
            }
        }
    }
}