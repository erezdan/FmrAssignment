using System;
using System.Globalization;
using System.Windows.Data;

namespace FmrClient.Converters
{
    public class DoubleToRoundedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                return d.ToString("F2", CultureInfo.InvariantCulture); // Always 2 decimal places
            }
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value?.ToString(), out double result))
                return result;

            return 0.0;
        }
    }
}

