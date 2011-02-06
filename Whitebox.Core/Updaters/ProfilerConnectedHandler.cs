using System;
using Whitebox.Core.Application;
using Whitebox.Messages;

namespace Whitebox.Core.Updaters
{
    class ProfilerConnectedHandler : IUpdateHandler<ProfilerConnectedMessage>
    {
        readonly IApplicationEventQueue _applicationEventQueue;

        public ProfilerConnectedHandler(IApplicationEventQueue applicationEventQueue)
        {
            _applicationEventQueue = applicationEventQueue;
        }

        public void UpdateFrom(ProfilerConnectedMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            _applicationEventQueue.Enqueue(new ProfilerConnectedEvent(message.ProcessName, message.ProcessId));
        }
    }
}
