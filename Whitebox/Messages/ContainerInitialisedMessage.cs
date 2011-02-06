using System;
using Whitebox.Model;

namespace Whitebox.Messages
{
    [Serializable]
    public class ContainerInitialisedMessage
    {
        readonly LifetimeScopeModel _rootLifetimeScope;

        public ContainerInitialisedMessage(LifetimeScopeModel rootLifetimeScope)
        {
            if (rootLifetimeScope == null) throw new ArgumentNullException("rootLifetimeScope");
            _rootLifetimeScope = rootLifetimeScope;
        }

        public LifetimeScopeModel RootLifetimeScope
        {
            get { return _rootLifetimeScope; }
        }
    }
}
