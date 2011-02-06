using System;

namespace Whitebox.Profiler.Util
{
    interface IDispatcher
    {
        void BeginInvoke(Action action);
    }
}