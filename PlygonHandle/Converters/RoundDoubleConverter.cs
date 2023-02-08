using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PlygonHandle.Converters
{
    public class RoundDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)parameter == 0)
                return Math.Round((double)value, MidpointRounding.ToZero);
            else
                return Math.Round((double)value, (int)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
}
