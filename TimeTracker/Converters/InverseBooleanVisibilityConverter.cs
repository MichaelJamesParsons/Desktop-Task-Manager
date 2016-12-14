using System;
using System.Windows.Data;

namespace TimeTracker.Converters
{
    [ValueConversion(typeof(string), typeof(bool))]
    public class InverseBooleanVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {

            var visibilityType = !((bool)value) ? "Visible" : "Collapsed";

            return visibilityType;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}