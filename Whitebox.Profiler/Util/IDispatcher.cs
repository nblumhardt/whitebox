using System;

namespace Whitebox.Profiler.Util
{
    interface IDispatcher
    {
        void Foreground(Action action);
        void Background(Action action);
    }
}