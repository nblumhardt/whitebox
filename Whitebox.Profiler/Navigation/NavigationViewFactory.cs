using System.Windows.Controls;
using Autofac.Features.Metadata;
using Autofac.Features.OwnedInstances;

namespace Whitebox.Profiler.Navigation
{
    delegate Meta<Owned<TView>, ViewMetadata> NavigationViewFactory<TView>(INavigator navigator)
        where TView : Control;

    delegate Meta<Owned<TView>, ViewMetadata> NavigationViewFactory<in TArg, TView>(TArg arg, INavigator navigator)
        where TView : Control;
}