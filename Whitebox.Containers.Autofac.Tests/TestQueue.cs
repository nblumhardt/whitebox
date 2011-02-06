using System.Collections.Generic;
using System.Linq;
using Whitebox.Connector;

namespace Whitebox.Containers.Autofac.Tests
{
    public class TestQueue : IReadQueue, IWriteQueue
    {
        readonly Queue<object> _messages = new Queue<object>();

        public bool TryDequeue(out object message)
        {
            if (!_messages.Any())
            {
                message = null;
                return false;
            }

            message = _messages.Dequeue();
            return true;
        }

        public void Enqueue(object message)
        {
            _messages.Enqueue(message);
        }

        public Queue<object> Messages { get { return _messages; } }
    }
}
