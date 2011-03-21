using System.Collections.Generic;
using System.Linq;
using Whitebox.Core.Application;

namespace Whitebox.Profiler.Features.ResolveOperations
{
    class SubResolveOperationViewModel : ViewModel
    {
        readonly InstanceLookupViewModel _rootInstanceLookup;
        readonly string _locationDescription;
        readonly bool _isRootOperation;

        public SubResolveOperationViewModel(ResolveOperation resolveOperation)
        {
            if (resolveOperation.RootInstanceLookup != null)
                _rootInstanceLookup = BuildInstanceLookupTree(resolveOperation.RootInstanceLookup);

            if (resolveOperation.CallingMethod != null)
                _locationDescription = resolveOperation.CallingMethod.DisplayName;

            _isRootOperation = resolveOperation.Parent == null;
        }

        public bool IsRootOperation
        {
            get { return _isRootOperation; }
        }

        public string LocationDescription
        {
            get { return _locationDescription; }
        }

        public InstanceLookupViewModel RootInstanceLookup
        {
            get { return _rootInstanceLookup; }
        }

        static InstanceLookupViewModel BuildInstanceLookupTree(InstanceLookup instanceLookup)
        {
            return new InstanceLookupViewModel(
                instanceLookup.Component.Description,
                Describe(instanceLookup.ActivationScope),
                !instanceLookup.SharedInstanceReused,
                instanceLookup.DependencyLookups.Select(BuildInstanceLookupTree));
        }

        static string Describe(LifetimeScope lifetimeScope)
        {
            if (lifetimeScope.Tag != null)
                return lifetimeScope.Tag;
            return "level " + lifetimeScope.Level;
        }
    }
}
