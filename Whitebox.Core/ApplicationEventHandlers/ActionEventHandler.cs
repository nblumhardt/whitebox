using Whitebox.Core.Application;
using Whitebox.Core.Session;

namespace Whitebox.Core.ApplicationEventHandlers
{
    class ActionEventHandler : IApplicationEventHandler<ActionEvent>
    {
        public void Handle(ActionEvent applicationEvent)
        {
            applicationEvent.Action.Invoke();
        }
    }
}
