using System;
using Whitebox.Model;

namespace Whitebox.Messages
{
    [Serializable]
    public class LifetimeScopeBeginningMessage
    {
        readonly LifetimeScopeModel _lifetimeScope;

        public LifetimeScopeBeginningMessage(LifetimeScopeModel lifetimeScope)
        {
            if (lifetimeScope == null) throw new ArgumentNullException("lifetimeScope");
            _lifetimeScope = lifetimeScope;
        }

        public LifetimeScopeModel LifetimeScope
        {
            get { return _lifetimeScope; }
        }
    }
}
