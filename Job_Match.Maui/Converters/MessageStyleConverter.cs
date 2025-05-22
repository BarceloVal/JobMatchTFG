using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Job_Match.Maui.Converters
{
    public class MessageStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isMine)
            {
                return Application.Current.Resources[isMine ? "MyMessageStyle" : "OtherMessageStyle"];
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
