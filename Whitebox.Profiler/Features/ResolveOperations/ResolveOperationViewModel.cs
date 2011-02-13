using System;
using System.Collections.ObjectModel;
using System.Linq;
using Whitebox.Core.Application;
using Whitebox.Profiler.Util;

namespace Whitebox.Profiler.Features.ResolveOperations
{
    class ResolveOperationViewModel : ViewModel
    {
        readonly ObservableCollection<SubResolveOperationViewModel> _subOperations = new ObservableCollection<SubResolveOperationViewModel>();

        public ResolveOperationViewModel(
            string resolveOperationId,
            IHistoricalItemStore<ResolveOperation> historicalItemStore,
            IDispatcher dispatcher)
        {
            if (resolveOperationId == null) throw new ArgumentNullException("resolveOperationId");
            if (historicalItemStore == null) throw new ArgumentNullException("historicalItemStore");

            dispatcher.Background(() =>
            {
                ResolveOperation resolveOperation;
                if (historicalItemStore.TryGetItem(resolveOperationId, out resolveOperation))
                {
                    var subOperations = Traverse.PreOrder(resolveOperation, r => r.SubOperations)
                        .Select(o => new SubResolveOperationViewModel(o))
                        .ToList();
                    
                    dispatcher.Foreground(() =>
                    {
                        foreach (var subResolveOperationViewModel in subOperations)
                            _subOperations.Add(subResolveOperationViewModel);
                    });
                }
            });
        }

        public ObservableCollection<SubResolveOperationViewModel> SubOperations { get { return _subOperations; } }
    }
}
