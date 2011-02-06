using System;

namespace Whitebox.Messages
{
    [Serializable]
    public class InstanceLookupCompletionBeginningMessage
    {
        readonly string _instanceLookupId;

        public InstanceLookupCompletionBeginningMessage(string instanceLookupId)
        {
            _instanceLookupId = instanceLookupId;
        }

        public string InstanceLookupId
        {
            get { return _instanceLookupId; }
        }
    }
}
