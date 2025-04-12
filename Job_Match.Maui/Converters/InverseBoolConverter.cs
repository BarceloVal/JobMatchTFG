using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Job_Match.Maui.Converters
{
    /// <summary>
    /// Convierte un valor booleano a su inverso.
    /// Útil en bindings, por ejemplo, para ocultar un elemento cuando una condición es verdadera.
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        /// <summary>
        /// Convierte un valor booleano a su valor contrario.
        /// </summary>
        /// <param name="value">El valor booleano original.</param>
        /// <param name="targetType">El tipo de destino del binding (no se usa).</param>
        /// <param name="parameter">Parámetro opcional del binding (no se usa).</param>
        /// <param name="culture">Cultura utilizada para la conversión (no se usa).</param>
        /// <returns>El valor contrario del booleano; si no es booleano, devuelve true por defecto.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return !b; // Invierte el booleano
            return true; // Si no es bool, devuelve true como fallback
        }

        /// <summary>
        /// Esta conversión no se utiliza en este caso (de UI a ViewModel).
        /// </summary>
        /// <exception cref="NotImplementedException">Lanzada siempre que se llama a ConvertBack.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // No implementado porque no se necesita revertir el valor en este caso
        }
    }
}
