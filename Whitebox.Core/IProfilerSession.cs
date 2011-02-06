using System;

namespace Whitebox.Core
{
    public interface IProfilerSession
    {
        // May be raised on any thread.
        event EventHandler<ApplicationConnectedEventArgs> Connected;

        void Start();

        event EventHandler<ApplicationDisconnectedEventArgs> Disconnected;

        void BeginInvoke(Action action);
    }
}
