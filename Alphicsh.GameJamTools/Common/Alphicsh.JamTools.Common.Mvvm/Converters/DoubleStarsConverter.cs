using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Alphicsh.JamTools.Common.Mvvm.Converters
{
    public class DoubleStarsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new GridLength((double)value, GridUnitType.Star);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((GridLength)value).Value;
        }
    }
}
