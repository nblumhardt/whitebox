using System;
using Whitebox.Core.Application;

namespace Whitebox.Core.Analytics.LifetimeDisposal
{
    class LifetimeDisposalDetector : IApplicationEventHandler<ItemCompletedEvent<LifetimeScope>>
    {
        readonly IApplicationEventQueue _applicationEventQueue;

        public LifetimeDisposalDetector(IApplicationEventQueue applicationEventQueue)
        {
            if (applicationEventQueue == null) throw new ArgumentNullException("applicationEventQueue");
            _applicationEventQueue = applicationEventQueue;
        }

        public void Handle(ItemCompletedEvent<LifetimeScope> applicationEvent)
        {
            var lifetimeScope = applicationEvent.Item;
            var messageEvent = new MessageEvent(MessageRelevance.Information, "Lifetime scope disposed", lifetimeScope.Description);
            _applicationEventQueue.Enqueue(messageEvent);
        }
    }
}