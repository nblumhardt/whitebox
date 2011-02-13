using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Autofac;
using Whitebox.Core;
using Whitebox.Core.Application;
using Whitebox.Core.Session;
using Whitebox.Profiler.Features.Analysis;
using Whitebox.Profiler.Features.Components;
using Whitebox.Profiler.Features.Events;
using Whitebox.Profiler.Features.Waiting;
using Whitebox.Profiler.Navigation;
using Whitebox.Profiler.Transitions;
using Whitebox.Profiler.Util;

namespace Whitebox.Profiler.Features.Session
{
    sealed class SessionViewModel :
        ViewModel,
        IApplicationEventHandler<ProfilerConnectedEvent>,
        INavigator,
        IDisposable
    {
        readonly IProfilerSession _profilerSession;
        readonly IApplicationEventBus _eventBus;
        readonly IDispatcher _dispatcher;
        readonly IComponentContext _componentContext;
        string _processDescription;
        readonly NavigationJournal _navigationJournal = new NavigationJournal();
        SlideDirection _slideDirection = SlideDirection.Forward;
        bool _isConnected;

        public SessionViewModel(
            IProfilerSession profilerSession, 
            IApplicationEventBus eventBus,
            IDispatcher dispatcher,
            IComponentContext componentContext)
        {
            if (profilerSession == null) throw new ArgumentNullException("profilerSession");
            if (eventBus == null) throw new ArgumentNullException("eventBus");
            if (componentContext == null) throw new ArgumentNullException("componentContext");
            _profilerSession = profilerSession;
            _eventBus = eventBus;
            _dispatcher = dispatcher;
            _componentContext = componentContext;
            NavigateBack = new RelayCommand(GoBack, () => _navigationJournal.CanGoBack);
            GoToEvents = new RelayCommand(Navigate<EventsView>, () => _isConnected);
            GoToComponents = new RelayCommand(Navigate<ComponentsView>, () => _isConnected);
            GoToAnalysis = new RelayCommand(Navigate<AnalysisView>, () => _isConnected);

            _navigationJournal.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Current")
                {
                    NotifyPropertyChanged("CurrentViewContent");
                    NotifyPropertyChanged("Title");
                    NotifyPropertyChanged("PreviousTitle");
                }
            };

            eventBus.Subscribe(this);
            Navigate<WaitingView>();
        }

        public event EventHandler<SessionConnectedEventArgs> Connected = delegate { };

        public SlideDirection SlideDirection
        {
            get { return _slideDirection; }
            set
            {
                if (value == _slideDirection) return;
                _slideDirection = value;
                NotifyPropertyChanged("SlideDirection");
            }
        }

        public void Handle(ProfilerConnectedEvent e)
        {
            _dispatcher.Foreground(() =>
            {
                _isConnected = true;
                ProcessDescription = e.Name + " (" + e.Id + ")";
                Connected(this, new SessionConnectedEventArgs(e.Name, e.Id));                
                Navigate<EventsView>();
                _navigationJournal.ClearHistory();
            });
        }

        public string ProcessDescription
        {
            get { return _processDescription; }
            set 
            {
                if (value == _processDescription) return;
                _processDescription = value;
                NotifyPropertyChanged("ProcessDescription");
            }
        }

        NavigationEntry CurrentViewEntry
        {
            get
            {
                return _navigationJournal.Current;
            }
        }

        public Control CurrentViewContent
        {
            get { return CurrentViewEntry.Content; }
        }

        public string Title
        {
            get { return CurrentViewEntry.Title; }
        }

        public void Navigate<TView>()
            where TView : Control
        {
            var viewFactory = _componentContext.Resolve<NavigationViewFactory<TView>>();
            Navigate(viewFactory(this));
        }

        public void Navigate<TArg, TView>(TArg arg) 
            where TView : Control
        {
            var viewFactory = _componentContext.Resolve<NavigationViewFactory<TArg, TView>>();
            Navigate(viewFactory(arg, this));
        }

        void Navigate<TView>(Autofac.Features.Metadata.Meta<Autofac.Features.OwnedInstances.Owned<TView>, IViewMetadata> view)
            where TView : Control
        {
            Navigate(new NavigationEntry(view.Value.Value, view.Value, view.Metadata.Title));
        }

        public void Navigate(NavigationEntry navigationEntry)
        {
            if (navigationEntry == null) throw new ArgumentNullException("navigationEntry");

            SlideDirection = SlideDirection.Forward;
            _navigationJournal.Current = navigationEntry;
            CommandManager.InvalidateRequerySuggested();
        }

        public void GoBack()
        {
            SlideDirection = SlideDirection.Back;
            _navigationJournal.GoBack();
        }

        public ICommand NavigateBack { get; private set; }

        public ICommand GoToEvents { get; private set; }

        public ICommand GoToComponents { get; private set; }

        public ICommand GoToAnalysis{ get; private set; }

        public string PreviousTitle
        {
            get { return _navigationJournal.BackHistory.Select(e => e.Title).FirstOrDefault(); }
        }

        public void Close()
        {
            _navigationJournal.Dispose();
        }

        public void Dispose()
        {
            Close();
            _eventBus.Unsubscribe(this);
        }

        public void Start()
        {
            _profilerSession.Start();
        }
    }
}
