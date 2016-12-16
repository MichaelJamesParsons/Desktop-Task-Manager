using System;
using System.Windows.Data;

namespace TimeTracker.Converters
{
    /// <summary>
    /// Hides the visibility of a UI element if a boolean value is true.
    /// </summary>
    [ValueConversion(typeof(string), typeof(bool))]
    public class InverseBooleanVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Set the UI elements visibility based on the value of the binded viewmodel property.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var visibilityType = !((bool)value) ? "Visible" : "Collapsed";
            return visibilityType;
        }

        /// <summary>
        /// Restores the original state of the UI element.
        /// 
        /// Not applicable with this converter.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}