using System;
using Whitebox.Model;

namespace Whitebox.Messages
{
    [Serializable]
    public class RegistrationSourceAddedMessage
    {
        readonly RegistrationSourceModel _registrationSource;

        public RegistrationSourceAddedMessage(RegistrationSourceModel registrationSource)
        {
            if (registrationSource == null) throw new ArgumentNullException("registrationSource");
            _registrationSource = registrationSource;
        }

        public RegistrationSourceModel RegistrationSource
        {
            get { return _registrationSource; }
        }
    }
}
