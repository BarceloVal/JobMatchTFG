using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Job_Match.Maui.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value?.ToString()?.ToLower();

            return status switch
            {
                "abierta" => Colors.Green,
                "en proceso de selección" => Colors.Goldenrod,
                "finalizada" => Colors.Red,
                _ => Colors.Black // color por defecto
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
