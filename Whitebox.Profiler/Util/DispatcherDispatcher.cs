using System;
using System.Windows.Threading;

namespace Whitebox.Profiler.Util
{
    class DispatcherDispatcher : IDispatcher
    {
        readonly Dispatcher _dispatcher;

        public DispatcherDispatcher(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void BeginInvoke(Action action)
        {
            _dispatcher.BeginInvoke(action);
        }
    }
}