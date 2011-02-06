using System.Collections.Generic;
using Whitebox.Core.Application;

namespace Whitebox.Profiler.Features.ResolveOperations
{
    interface IHistoricalItemStore<TItem>
        where TItem : IApplicationItem
    {
        bool TryGetItem(string itemId, out TItem item);
        IEnumerable<TItem> GetItems();
    }
}