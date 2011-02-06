using System.Windows.Controls;

namespace Whitebox.Profiler.Navigation
{
    interface INavigator
    {
        void Navigate<TView>() where TView : Control;
        void Navigate<TArg, TView>(TArg arg) where TView : Control;
        void Navigate(NavigationEntry navigationEntry);
        void GoBack();
    }
}
