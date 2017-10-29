using System;
using System.Globalization;
using System.Windows.Data;

namespace RedFlag.Converters
{
    public class BoolToYesNo : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value ? "Yes" : "No");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Value is never asigned back to object
            return null;
        }
    }
}