using System;
using System.Collections.Generic;
using Whitebox.Core.Application;
using Whitebox.Profiler.Display;
using Whitebox.Profiler.Util;

namespace Whitebox.Profiler.Features.ComponentDetail
{
    class ComponentDetailViewModel : ViewModel
    {
        string _description;
        IDictionary<string, string> _metadata;
        IEnumerable<string> _services;

        public ComponentDetailViewModel(string componentId, IDispatcher dispatcher, IActiveItemRepository<Component> components)
        {
            dispatcher.Background(() =>
            {
                Component component;
                if (!components.TryGetItem(componentId, out component))
                    throw new ArgumentException("Unknown component.");

                var description = component.Description;
                var services = component.DescribeServices();
                var metadata = component.Metadata;

                dispatcher.Foreground(() =>
                {
                    Description = description;
                    Metadata = metadata;
                    Services = services;
                });
            });
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value) return;
                _description = value;
                NotifyPropertyChanged("Description");
            }
        }

        public IDictionary<string, string> Metadata
        {
            get { return _metadata; }
            set
            {
                if (_metadata == value) return;
                _metadata = value;
                NotifyPropertyChanged("Metadata");
            }
        }

        public IEnumerable<string> Services
        {
            get { return _services; }
            set
            {
                if (_services == value) return;
                _services = value;
                NotifyPropertyChanged("Services");
            }
        }
    }
}
