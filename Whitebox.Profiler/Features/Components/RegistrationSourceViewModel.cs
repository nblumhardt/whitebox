using System.Windows.Input;
using Whitebox.Profiler.Navigation;
using Whitebox.Profiler.Util;

namespace Whitebox.Profiler.Features.Components
{
    class RegistrationSourceViewModel
    {
        readonly string _description;

        public RegistrationSourceViewModel(string resolveOperationId, INavigator navigator, string description)
        {
            _description = description;
            GoToComponent = new RelayCommand(() => { });
        }

        public string Description
        {
            get { return _description; }
        }

        public ICommand GoToComponent { get; private set; }
    }
}
