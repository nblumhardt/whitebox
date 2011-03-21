using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Whitebox.Profiler.Features.ResolveOperations
{
    class InstanceLookupBorderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var newInstance = (bool) value;

            return newInstance ? Brushes.Black : Brushes.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
