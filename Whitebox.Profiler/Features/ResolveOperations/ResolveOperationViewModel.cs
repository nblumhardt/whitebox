using System;
using System.Collections.ObjectModel;
using System.Linq;
using Whitebox.Core;
using Whitebox.Core.Application;
using Whitebox.Profiler.Util;

namespace Whitebox.Profiler.Features.ResolveOperations
{
    class ResolveOperationViewModel : ViewModel
    {
        readonly ObservableCollection<SubResolveOperationViewModel> _subOperations = new ObservableCollection<SubResolveOperationViewModel>();

        public ResolveOperationViewModel(
            string resolveOperationId,
            IProfilerSession profilerSession,
            IHistoricalItemStore<ResolveOperation> historicalItemStore,
            IDispatcher dispatcher)
        {
            if (resolveOperationId == null) throw new ArgumentNullException("resolveOperationId");
            if (profilerSession == null) throw new ArgumentNullException("profilerSession");
            if (historicalItemStore == null) throw new ArgumentNullException("historicalItemStore");

            profilerSession.BeginInvoke(() =>
            {
                ResolveOperation resolveOperation;
                if (historicalItemStore.TryGetItem(resolveOperationId, out resolveOperation))
                {
                    var subOperations = Traverse.PreOrder(resolveOperation, r => r.SubOperations)
                        .Select(o => new SubResolveOperationViewModel(o))
                        .ToList();
                    
                    dispatcher.BeginInvoke(() =>
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
