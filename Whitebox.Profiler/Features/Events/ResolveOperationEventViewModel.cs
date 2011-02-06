using System;
using System.Windows.Input;
using Whitebox.Profiler.Features.ResolveOperations;
using Whitebox.Profiler.Navigation;
using Whitebox.Profiler.Util;

namespace Whitebox.Profiler.Features.Events
{
    class ResolveOperationEventViewModel : EventViewModel
    {
        readonly string _resolveOperationId;
        readonly INavigator _navigator;
        string _componentDescription;
        string _locationDescription;
        readonly ICommand _showEvent;

        public ResolveOperationEventViewModel(string resolveOperationId, INavigator navigator)
        {
            if (resolveOperationId == null) throw new ArgumentNullException("resolveOperationId");
            if (navigator == null) throw new ArgumentNullException("navigator");
            _resolveOperationId = resolveOperationId;
            _navigator = navigator;
            _showEvent = new RelayCommand(OnShowEvent);
        }

        public string ResolveOperationId
        {
            get { return _resolveOperationId; }
        }

        public string Title
        {
            get { return "Resolved " + ComponentDescription; }
        }

        public string ComponentDescription
        {
            get { return _componentDescription; }
            set
            {
                if (value == _componentDescription) return;
                _componentDescription = value;
                NotifyPropertyChanged("ComponentDescription");
            }
        }

        public string LocationDescription
        {
            get { return _locationDescription; }
            set
            {
                if (value == _locationDescription) return;
                _locationDescription = value;
                NotifyPropertyChanged("LocationDescription");
            }
        }

        public override ICommand ShowEvent { get { return _showEvent; } }

        void OnShowEvent()
        {
            _navigator.Navigate<string, ResolveOperationView>(_resolveOperationId);
        }

        public override string Icon { get { return @"..\..\Resources\ResolveOperation-24.png"; }}
    }
}
