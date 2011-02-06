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
                Describe(instanceLookup),
                instanceLookup.DependencyLookups.Select(BuildInstanceLookupTree));
        }

        static string Describe(LifetimeScope lifetimeScope)
        {
            if (lifetimeScope.Tag != null)
                return lifetimeScope.Tag;
            return "level " + lifetimeScope.Level;
        }

        static string Describe(InstanceLookup instanceLookup)
        {
            var labels = new List<string>();

            if (!instanceLookup.SharedInstanceReused)
                labels.Add("new");

            if (instanceLookup.Dependent == null || instanceLookup.ActivationScope != instanceLookup.Dependent.ActivationScope)
                labels.Add(Describe(instanceLookup.ActivationScope));

            var result = instanceLookup.Component.Description;
            if (labels.Count != 0)
            {
                result += " (";
                result += string.Join(", ", labels);
                result += ")";
            }

            return result;
        }
    }
}
