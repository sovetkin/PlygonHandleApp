using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PlygonHandle.Converters
{
    public class NullToUnsetValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return DependencyProperty.UnsetValue;

            if (value is PointCollection obj)
                return obj.Count == 0 ? DependencyProperty.UnsetValue : value;

            return value is Point p ? p.X == -1 && p.Y == -1 ? DependencyProperty.UnsetValue : value : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
}
