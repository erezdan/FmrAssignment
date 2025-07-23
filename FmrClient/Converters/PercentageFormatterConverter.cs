using System;
using System.Globalization;
using System.Windows.Data;

namespace FmrClient.Converters
{
    public class PercentageFormatterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                string formatted = "%" + Math.Abs(d).ToString("0.##");
                return d >= 0 ? formatted : formatted + "-";
            }

            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
