using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TimeTracker.Converters
{
    /// <summary>
    /// Hides a UI element if the binded property contains a
    /// null value.
    /// </summary>
    class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
