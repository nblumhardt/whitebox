using System;
using System.Windows;

namespace Whitebox.Profiler.Behaviours
{
    public static class LoadUnload
    {
        public static readonly DependencyProperty AwareProperty = DependencyProperty.RegisterAttached(
            "Aware", typeof(bool),
            typeof(LoadUnload),
            new FrameworkPropertyMetadata(false, OnAwareChanged));

        public static void SetAware(FrameworkElement element, Boolean value)
        {
            element.SetValue(AwareProperty, value);
        }

        public static Boolean GetAware(FrameworkElement element)
        {
            return (Boolean)element.GetValue(AwareProperty);
        }

        public static void OnAwareChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var element = (FrameworkElement)obj;
 
            element.DataContextChanged += (sender, e) =>
            {
                if (!element.IsLoaded)
                    return;

                if (e.OldValue is ILoadUnloadAware)
                    ((ILoadUnloadAware)e.OldValue).OnUnload(element);

                if (e.NewValue is ILoadUnloadAware)
                    ((ILoadUnloadAware)e.OldValue).OnLoad(element);
            };
 
            element.Loaded += (sender, e) =>
            {
                var aware = element.GetValue(FrameworkElement.DataContextProperty) as ILoadUnloadAware;
 
                if (aware != null)
                    aware.OnLoad(element);
            };
 
            element.Unloaded += (sender, e) =>
            {
                var aware = element.GetValue(FrameworkElement.DataContextProperty) as ILoadUnloadAware;

                if (aware != null)
                    aware.OnUnload(element);
            };
        }
    }
}
