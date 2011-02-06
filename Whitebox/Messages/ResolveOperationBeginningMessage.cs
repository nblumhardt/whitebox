using System;
using Whitebox.Model;

namespace Whitebox.Messages
{
    [Serializable]
    public class ResolveOperationBeginningMessage
    {
        readonly ResolveOperationModel _resolveOperation;

        public ResolveOperationBeginningMessage(ResolveOperationModel resolveOperation)
        {
            if (resolveOperation == null) throw new ArgumentNullException("resolveOperation");
            _resolveOperation = resolveOperation;
        }

        public ResolveOperationModel ResolveOperation
        {
            get { return _resolveOperation; }
        }
    }
}
