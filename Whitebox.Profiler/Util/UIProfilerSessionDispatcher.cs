using System;
using System.Windows.Threading;
using Whitebox.Core;

namespace Whitebox.Profiler.Util
{
    class UIProfilerSessionDispatcher : IDispatcher
    {
        readonly Dispatcher _dispatcher;
        readonly IProfilerSession _profilerSession;

        public UIProfilerSessionDispatcher(Dispatcher dispatcher, IProfilerSession profilerSession)
        {
            _dispatcher = dispatcher;
            _profilerSession = profilerSession;
        }

        public void Foreground(Action action)
        {
            _dispatcher.BeginInvoke(action);
        }

        public void Background(Action action)
        {
            _profilerSession.BeginInvoke(action);
        }
    }
}