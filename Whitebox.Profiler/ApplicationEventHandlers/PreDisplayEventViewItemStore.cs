using System;
using System.Collections.Generic;
using System.Linq;
using Whitebox.Core.Analytics;
using Whitebox.Core.Application;
using Whitebox.Profiler.Features.Events;

namespace Whitebox.Profiler.ApplicationEventHandlers
{
    class PreDisplayEventViewItemStore :
        IPreDisplayEventViewItemStore,
        IApplicationEventHandler<MessageEvent>,
        IApplicationEventHandler<ItemCreatedEvent<ResolveOperation>>,
        IApplicationEventHandler<ItemCompletedEvent<ResolveOperation>>
    {
        bool _saving = true;
        readonly object _synchRoot = new object();
        readonly Queue<Action<object>> _replayQueue = new Queue<Action<object>>();

        public void Handle(MessageEvent applicationEvent)
        {
            Enqueue(applicationEvent);
        }

        public void Handle(ItemCreatedEvent<ResolveOperation> applicationEvent)
        {
            Enqueue(applicationEvent);
        }

        public void Handle(ItemCompletedEvent<ResolveOperation> applicationEvent)
        {
            Enqueue(applicationEvent);
        }

        void Enqueue<T>(T applicationEvent)
        {
            lock (_synchRoot)
            {
                if (_saving)
                    _replayQueue.Enqueue(h => ((IApplicationEventHandler<T>)h).Handle(applicationEvent));
            }
        }

        public void Unload(object handler)
        {
            lock (_synchRoot)
            {
                _saving = false;
                while (_replayQueue.Any())
                {
                    var action = _replayQueue.Dequeue();
                    action(handler);
                }
            }
        }
    }
}
