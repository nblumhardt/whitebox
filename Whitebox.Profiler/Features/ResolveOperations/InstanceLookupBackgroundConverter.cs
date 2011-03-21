using System;
using System.Globalization;
using System.Windows.Data;

namespace Whitebox.Profiler.Features.ResolveOperations
{
    class InstanceLookupBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vm = value as InstanceLookupViewModel;
            if (vm == null)
                return null;

            var key = (LifetimeColorKey) parameter;

            var baseBrush = key.GetBrush(vm.LifetimeScopeDescription);
            if (vm.NewInstanceCreated)
                return baseBrush;

            var lightened = baseBrush.Clone();
            lightened.Opacity = 0.5;
            return lightened;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
