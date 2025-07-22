using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FmrClient.Converters
{
    public class PercentageToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double percentage)
            {
                if (percentage > 0)
                    return Brushes.LightGreen;
                else if (percentage < 0)
                    return Brushes.IndianRed;
            }

            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
