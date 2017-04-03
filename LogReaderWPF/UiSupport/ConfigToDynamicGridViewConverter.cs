using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using LogReader;

namespace LogReaderWPF
{
    public class ConfigToDynamicGridViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var config = value as LogContext;
            if (config != null)
            {
                var grdiView = new GridView();
                grdiView.Columns.Add(new GridViewColumn {Header = "Id", DisplayMemberBinding = new Binding("Id")});

                for (int column = 0; column < config.Parser.ColumnCount; column++)
                {
                    var gridViewColumn = new GridViewColumn {Header = "Column" + column};

                    gridViewColumn.CellTemplate = GetDataTemplate(column.ToString());

                    grdiView.Columns.Add(gridViewColumn);

                }
                return grdiView;
            }
            return Binding.DoNothing;
        }

        private static DataTemplate GetDataTemplate(string columnName)
        {
            DataTemplate cell = new DataTemplate(); // create a datatemplate
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(TextBox));
            Binding binding = new Binding("[" + columnName + "]");
            binding.Mode = BindingMode.OneWay;
            factory.SetBinding(TextBox.TextProperty, binding);
            factory.SetValue(TextBox.IsReadOnlyProperty, true);
            factory.SetValue(TextBox.BorderBrushProperty, new SolidColorBrush() {Opacity = 1});
            factory.SetValue(TextBox.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            factory.SetBinding(TextBox.ForegroundProperty, new Binding("Foreground"));
            factory.SetBinding(TextBox.BackgroundProperty, new Binding("Background"));
            factory.SetBinding(TextBox.FontFamilyProperty, new Binding("FontFamily"));
            factory.SetBinding(TextBox.FontSizeProperty, new Binding("FontSize"));
            cell.VisualTree = factory;

            return cell;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}