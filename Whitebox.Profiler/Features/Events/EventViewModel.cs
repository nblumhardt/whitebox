using System.Windows.Input;

namespace Whitebox.Profiler.Features.Events
{
    abstract class EventViewModel : ViewModel
    {
        public abstract ICommand ShowEvent { get; }

        public abstract string Icon { get; }
    }
}
