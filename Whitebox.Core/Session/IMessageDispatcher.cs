using Whitebox.Connector;

namespace Whitebox.Core.Session
{
    interface IMessageDispatcher
    {
        void DispatchMessages(IReadQueue readQueue);
    }
}
