using System;
using System.Collections.Generic;
using System.Linq;

namespace Whitebox.Profiler.Features.ResolveOperations
{
    class InstanceLookupViewModel : ViewModel
    {
        readonly string _componentDescription;
        readonly string _lifetimeScopeDescription;
        readonly bool _newInstanceCreated;
        readonly IEnumerable<InstanceLookupViewModel> _dependencyLookups;

        public InstanceLookupViewModel(string componentDescription, string lifetimeScopeDescription, bool newInstanceCreated, IEnumerable<InstanceLookupViewModel> dependencyLookups)
        {
            if (componentDescription == null) throw new ArgumentNullException("componentDescription");
            if (lifetimeScopeDescription == null) throw new ArgumentNullException("lifetimeScopeDescription");
            if (dependencyLookups == null) throw new ArgumentNullException("dependencyLookups");
            _componentDescription = componentDescription;
            _lifetimeScopeDescription = lifetimeScopeDescription;
            _newInstanceCreated = newInstanceCreated;
            _dependencyLookups = dependencyLookups.ToArray();
        }

        public bool NewInstanceCreated
        {
            get { return _newInstanceCreated; }
        }

        public string LifetimeScopeDescription
        {
            get { return _lifetimeScopeDescription; }
        }

        public IEnumerable<InstanceLookupViewModel> DependencyLookups
        {
            get { return _dependencyLookups; }
        }

        public string ComponentDescription
        {
            get { return _componentDescription; }
        }

        public string Tags
        {
            get
            {
                var result = new List<string>();
                if (NewInstanceCreated)
                    result.Add("new");
                result.Add(LifetimeScopeDescription);
                return string.Join(", ", result);
            }
        }
    }
}
