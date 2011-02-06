using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Whitebox.Core.Application;
using Whitebox.Profiler.Features.ResolveOperations;

namespace Whitebox.Profiler.ApplicationEventHandlers
{
    class HistoricalItemStore<TItem> :
            IApplicationEventHandler<ItemCreatedEvent<TItem>>,
            IHistoricalItemStore<TItem>
        where TItem : IApplicationItem
    {
        readonly ConcurrentDictionary<string, TItem> _items = new ConcurrentDictionary<string, TItem>();

        public void Handle(ItemCreatedEvent<TItem> applicationEvent)
        {
            if (!_items.TryAdd(applicationEvent.Item.Id, applicationEvent.Item))
                throw new InvalidOperationException("Item operation with the specified id already added.");
        }

        public bool TryGetItem(string itemId, out TItem item)
        {
            return _items.TryGetValue(itemId, out item);
        }

        public IEnumerable<TItem> GetItems()
        {
            return _items.ToArray().Select(i => i.Value);
        }
    }
}
