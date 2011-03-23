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
        string _sharing;
        string _lifetime;
        string _ownership;
        string _activator;
        string _target;
        string _id;

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
                var sharing = component.Sharing;
                var lifetime = component.Lifetime;
                var activator = component.Activator;
                var ownership = component.Ownership;
                var target = component.TargetComponentId;
                var id = component.Id;

                dispatcher.Foreground(() =>
                {
                    Description = description;
                    Metadata = metadata;
                    Services = services;
                    Sharing = sharing.ToString();
                    Lifetime = lifetime.ToString();
                    Activator = activator.ToString();
                    Ownership = ownership.ToString();
                    Target = target;
                    Id = id;
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

        public string Sharing
        {
            get { return _sharing; }
            set
            {
                if (_sharing == value) return;
                _sharing = value;
                NotifyPropertyChanged("Sharing");
            }
        }

        public string Lifetime
        {
            get { return _lifetime; }
            set
            {
                if (_lifetime == value) return;
                _lifetime = value;
                NotifyPropertyChanged("Lifetime");
            }
        }

        public string Ownership
        {
            get { return _ownership; }
            set
            {
                if (_ownership == value) return;
                _ownership = value;
                NotifyPropertyChanged("Ownership");
            }
        }

        public string Activator
        {
            get { return _activator; }
            set
            {
                if (_activator == value) return;
                _activator = value;
                NotifyPropertyChanged("Activator");
            }
        }

        public string Target
        {
            get { return _target; }
            set
            {
                if (_target == value) return;
                _target = value;
                NotifyPropertyChanged("Target");
            }
        }

        public string Id
        {
            get { return _id; }
            set
            {
                if (_id == value) return;
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }
    }
}

