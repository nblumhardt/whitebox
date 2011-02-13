using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Whitebox.Core;
using Whitebox.Core.Application;
using Whitebox.Core.Session;
using Whitebox.Profiler.Display;
using Whitebox.Profiler.Features.ResolveOperations;
using Whitebox.Profiler.Navigation;
using Whitebox.Profiler.Util;
using Component = Whitebox.Core.Application.Component;

namespace Whitebox.Profiler.Features.Components
{
    sealed class ComponentsViewModel :
        ViewModel,
        IApplicationEventHandler<ItemCreatedEvent<Component>>,
        IApplicationEventHandler<ItemCreatedEvent<RegistrationSource>>,
        IDisposable
    {
        readonly INavigator _navigator;
        readonly IApplicationEventBus _applicationEventBus;
        readonly IDispatcher _dispatcher;
        readonly ObservableCollection<object> _components = new ObservableCollection<object>();
        readonly CollectionView _filteredSortedComponents;

        public ComponentsViewModel(
            INavigator navigator,
            IHistoricalItemStore<Component> components,
            IHistoricalItemStore<RegistrationSource> registrationSources, 
            IApplicationEventBus applicationEventBus, 
            IDispatcher dispatcher)
        {
            _navigator = navigator;
            _applicationEventBus = applicationEventBus;
            _dispatcher = dispatcher;
            _filteredSortedComponents = new ListCollectionView(_components);
            _filteredSortedComponents.SortDescriptions.Add(new SortDescription("Description", ListSortDirection.Ascending));

            dispatcher.Background(() =>
            {
                _applicationEventBus.Subscribe(this);
                var vms = new List<object>();
                vms.AddRange(components.GetItems().Select(CreateViewModel));
                vms.AddRange(registrationSources.GetItems().Select(CreateViewModel));
                dispatcher.Foreground(() =>
                {
                    foreach (var vm in vms)
                        _components.Add(vm);
                });
            });
        }

        public IEnumerable Components { get { return _filteredSortedComponents; } }

        public void Handle(ItemCreatedEvent<Component> applicationEvent)
        {
            var vm = CreateViewModel(applicationEvent.Item);
            _dispatcher.Foreground(() => _components.Add(vm));
        }

        ComponentViewModel CreateViewModel(Component component)
        {
            return new ComponentViewModel(component.Id, _navigator, component.Description, component.DescribeServices());
        }

        RegistrationSourceViewModel CreateViewModel(RegistrationSource registrationSource)
        {
            return new RegistrationSourceViewModel(registrationSource.Id, _navigator, registrationSource.Description);
        }

        public void Dispose()
        {
            _applicationEventBus.Unsubscribe(this);
        }

        public void Handle(ItemCreatedEvent<RegistrationSource> applicationEvent)
        {
            var vm = CreateViewModel(applicationEvent.Item);
            _dispatcher.Foreground(() => _components.Add(vm));
        }
    }
}
