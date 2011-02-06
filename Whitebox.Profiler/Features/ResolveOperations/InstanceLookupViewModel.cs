using System;
using System.Collections.Generic;
using System.Linq;

namespace Whitebox.Profiler.Features.ResolveOperations
{
    class InstanceLookupViewModel : ViewModel
    {
        readonly string _componentDescription;
        readonly IEnumerable<InstanceLookupViewModel> _dependencyLookups;

        public InstanceLookupViewModel(string componentDescription, IEnumerable<InstanceLookupViewModel> dependencyLookups)
        {
            if (componentDescription == null) throw new ArgumentNullException("componentDescription");
            if (dependencyLookups == null) throw new ArgumentNullException("dependencyLookups");
            _componentDescription = componentDescription;
            _dependencyLookups = dependencyLookups.ToArray();
        }

        public IEnumerable<InstanceLookupViewModel> DependencyLookups
        {
            get { return _dependencyLookups; }
        }

        public string ComponentDescription
        {
            get { return _componentDescription; }
        }
    }
}
