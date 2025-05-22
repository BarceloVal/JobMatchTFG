using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Job_Match.Maui.Converters
{
    public class NullOrZeroToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intVal)
                return intVal > 0;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}