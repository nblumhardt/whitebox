using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Whitebox.Profiler.Navigation;
using Whitebox.Profiler.Util;

namespace Whitebox.Profiler.Features.Components
{
    class ComponentViewModel
    {
        readonly string _description;
        readonly string _services;

        public ComponentViewModel(string componentId, INavigator navigator, string componentDescription, IEnumerable<string> serviceDescriptions)
        {
            _description = componentDescription;
            if (serviceDescriptions.Any())
                _services += " –o " + string.Join(", ", serviceDescriptions);
            GoToComponent = new RelayCommand(() => { });
        }

        public string Description
        {
            get { return _description; }
        }

        public string Services
        {
            get { return _services; }
        }

        public ICommand GoToComponent { get; private set; }
    }
}
