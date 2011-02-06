using System.Windows;

namespace Whitebox.Profiler.Behaviours
{
    interface ILoadUnloadAware
    {
        void OnLoad(FrameworkElement frameworkElement);
        void OnUnload(FrameworkElement frameworkElement);
    }
}
