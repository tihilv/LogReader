using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using LogReader;

namespace LogReaderWPF
{
    public class ConfigToDynamicGridViewConverter : IValueConverter
    {
        Brush b = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff0000"));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var config = value as LogContext;
            if (config != null)
            {
                var grdiView = new GridView();
                grdiView.Columns.Add(new GridViewColumn {Header = "Id", DisplayMemberBinding = new Binding("Id")});
                for (int column = 0; column < config.Parser.ColumnCount; column++)
                {
                    var binding = new Binding("["+column+"]");
                    grdiView.Columns.Add(new GridViewColumn { Header = "Column"+column, DisplayMemberBinding = binding});
                }
                return grdiView;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}