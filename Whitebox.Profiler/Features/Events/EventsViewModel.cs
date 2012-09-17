using System;
using System.Linq;
using System.Windows.Input;
using Whitebox.Core.Analytics;
using Whitebox.Core.Application;
using Whitebox.Core.Session;
using Whitebox.Profiler.Navigation;
using Whitebox.Profiler.Util;
using System.Collections.ObjectModel;

namespace Whitebox.Profiler.Features.Events
{
    sealed class EventsViewModel : 
        ViewModel, 
        IApplicationEventHandler<MessageEvent>,
        IApplicationEventHandler<ItemCreatedEvent<ResolveOperation>>,
        IApplicationEventHandler<ItemCompletedEvent<ResolveOperation>>,
        IDisposable
    {
        readonly IDispatcher _dispacher;
        readonly IApplicationEventBus _eventBus;
        readonly INavigator _navigator;
        readonly ObservableCollection<EventViewModel> _events = new ObservableCollection<EventViewModel>();
        bool _isPaused;

        public EventsViewModel(IDispatcher dispacher, IApplicationEventBus eventBus, INavigator navigator, IPreDisplayEventViewItemStore preDisplayEventViewItemStore)
        {
            if (dispacher == null) throw new ArgumentNullException("dispacher");
            if (eventBus == null) throw new ArgumentNullException("eventBus");
            if (navigator == null) throw new ArgumentNullException("navigator");
            if (preDisplayEventViewItemStore == null) throw new ArgumentNullException("preDisplayEventViewItemStore");
            _dispacher = dispacher;
            _eventBus = eventBus;
            _navigator = navigator;
            PauseResume = new RelayCommand(OnPauseResume);
            Clear = new RelayCommand(OnClear);
            _dispacher.Background(() =>
            {
                _eventBus.Subscribe(this);
                preDisplayEventViewItemStore.Unload(this);
            });
        }

        public ICommand PauseResume { get; private set; }

        public ICommand Clear { get; private set; }

        public bool IsPaused
        {
            get { return _isPaused; }
            private set
            {
                if (value == _isPaused) return;
                _isPaused = value;
                NotifyPropertyChanged("IsPaused");
            }
        }

        void OnPauseResume()
        {
            IsPaused = !IsPaused;
        }

        void OnClear()
        {
            _events.Clear();
        }

        public ObservableCollection<EventViewModel> Events { get { return _events; } }

        public void Handle(MessageEvent applicationEvent)
        {
            if (IsPaused)
                return;

            var vm = new MessageEventViewModel(applicationEvent.Relevance, applicationEvent.Title, applicationEvent.Message);
            _dispacher.Foreground(() => _events.Insert(0, vm));
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe(this);
        }

        public void Handle(ItemCreatedEvent<ResolveOperation> applicationEvent)
        {
            if (IsPaused)
                return;

            if (applicationEvent.Item.Parent == null)
            {
                var vm = new ResolveOperationEventViewModel(applicationEvent.Item.Id, _navigator);

                _dispacher.Foreground(() => _events.Insert(0, vm));
            }
        }

        public void Handle(ItemCompletedEvent<ResolveOperation> applicationEvent)
        {
            var resolveOperationId = applicationEvent.Item.Id;
            var componentDescription = applicationEvent.Item.RootInstanceLookup.Component.Description;
            string locationDescription = null;
            if (applicationEvent.Item.CallingMethod != null)
                locationDescription = applicationEvent.Item.CallingMethod.DisplayName;

            _dispacher.Foreground(() =>
            {
                ResolveOperationEventViewModel resolveOperation;
                if (TryGetResolveOperationEvent(resolveOperationId, out resolveOperation))
                {
                    resolveOperation.ComponentDescription = componentDescription;
                    resolveOperation.LocationDescription = locationDescription;
                }
            });
        }

        bool TryGetResolveOperationEvent(string resolveOperationId, out ResolveOperationEventViewModel result)
        {
            result = Events.OfType<ResolveOperationEventViewModel>().SingleOrDefault(e => e.ResolveOperationId == resolveOperationId);
            return result != null;
        }
    }
}
