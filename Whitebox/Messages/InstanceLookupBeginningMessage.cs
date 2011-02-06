using System;
using Whitebox.Model;

namespace Whitebox.Messages
{
    [Serializable]
    public class InstanceLookupBeginningMessage
    {
        readonly InstanceLookupModel _instanceLookup;

        public InstanceLookupBeginningMessage(InstanceLookupModel instanceLookup)
        {
            _instanceLookup = instanceLookup;
        }

        public InstanceLookupModel InstanceLookup
        {
            get { return _instanceLookup; }
        }
    }
}
